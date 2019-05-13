using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace BS_Project.DAO
{
    public class CategoryDAO
    {
        private BSDBContext db = new BSDBContext();

        /// <summary>
        /// Lay danh sach category theo phan trang
        /// </summary>
        /// <param name="searchString">Chuỗi tìm kiếm</param>
        /// <param name="page">Trang mặc định khi người dùng load vào</param>
        /// <param name="pageSize">Số record trong một trang</param>
        /// <returns></returns>
        public IEnumerable<Category> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Category> model = db.Categories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = db.Categories.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderBy(x => x.CategoryID).ToPagedList(page, pageSize);
        }

        /// <summary>
        /// Lấy Category đã khi Insert, thực hiện Insert lên CSDL
        /// </summary>
        /// <param name="entity">Categoy đã Insert trên trang Index Category</param>
        /// <returns></returns>
        public int Insert(Category entity)
        {
            db.Categories.Add(entity);
            db.SaveChanges();
            return entity.CategoryID;
        }

        public bool CheckName(string name)
        {
            var cateDetail = db.Categories.Where(x => x.Name == name);
            if (cateDetail.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// See information detail of Category
        /// </summary>
        /// <param name="cateID"></param>
        /// <returns></returns>
        public Category ViewDetail(int cateID)
        {
            return db.Categories.Find(cateID);
        }

        /// <summary>
        /// Thực hiên việc Update Category đã chỉnh sửa
        /// </summary>
        /// <param name="entity">Category đã chỉnh sửa</param>
        /// <returns></returns>
        public bool Update(Category entity)
        {
            try
            {
                var category = db.Categories.Find(entity.CategoryID);
                category.Name = entity.Name;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Delete Category đã chon Có CategoryID = id
        /// </summary>
        /// <param name="id">CategoryID</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                var category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<string> ListName(string keyword)
        {
            return db.Categories.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
    }
}