using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using System.Data.SqlClient;

namespace BS_Project.DAO
{
    public class AuthorDAO
    {
        private BSDBContext db = new BSDBContext();

        /// <summary>
        /// Tạo ra danh sách có pagelist
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page">số trang hiện thị mặc định</param>
        /// <param name="pageSize">số cột chứa trong 1 page</param>
        /// <returns></returns>
        public IEnumerable<Author> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Author> model = db.Authors.Where(x => x.isDeleted == false);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = db.Authors.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderBy(x => x.AuthorID).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// Thêm vào 1 tác giả dựa trên ID
        /// </summary>
        /// <param name="author"></param>
        /// <returns>trả về ID của tác giả được chọn</returns>
        public int Insert(Author author, string imageURL)
        {
            db.Authors.Add(author);
            var image = new Image();
            image.ImageURL = imageURL;
            var imageID = db.Images.Add(image);
            db.SaveChanges();
            //câu lệnh sửa imageboolID của 1 tác giả
            string updateAuthorImage = "update Authors set ImageBoolID = " + imageID.ImageBoolID + " where AuthorID = " + author.AuthorID;
            db.Database.ExecuteSqlCommand(updateAuthorImage);

            db.SaveChanges();
            return author.AuthorID;
        }

        /// <summary>
        /// Tìm thông tin chi tiết của 1 tác giả
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Author ViewDetail(int id)
        {
            return db.Authors.Find(id);
        }

        /// <summary>
        /// update thông tin của tác giả
        /// </summary>
        /// <param name="author"></param>
        /// <returns> true nếu update thành công, fasle nếu update không thành công</returns>
        public bool Update(Author author, string imageURL)
        {
            try
            {
                var authorOld = db.Authors.Find(author.AuthorID);
                authorOld.Name = author.Name;
                authorOld.PenName = author.PenName;
                authorOld.Overview = author.Overview;

                //cập nhật URL ảnh trên database
                //authorOld.Image.ImageURL = imageURL;

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// xóa đi 1 tác giả
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                var author = db.Authors.Find(id);
                author.isDeleted = true;
                //db.Authors.Remove(author);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// Hàm lưu Sách mới vào AuthorsBooks
        /// </summary>
        /// <param name="bookID"></param>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public int InsertAuthorsBooks(int bookID, int AuthorID)
        {
            try
            {
                string queryStr = "Insert Into AuthorsBooks values ('" + AuthorID + "', " + bookID + ");";
                db.Database.ExecuteSqlCommand(queryStr);
                db.SaveChanges();
                return bookID;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

    }
}