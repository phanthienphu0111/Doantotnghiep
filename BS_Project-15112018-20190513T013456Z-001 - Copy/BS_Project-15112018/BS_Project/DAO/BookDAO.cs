using BS_Project.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS_Project.DAO
{
    public class BookDAO
    {
        private BSDBContext db = new BSDBContext();

        public Book BookDetail(int id)
        {
            return db.Books.Find(id);
        }

        /// <summary>
        /// Update thông tin của 1 cuốn sách
        /// </summary>
        /// <param name="book"></param>
        /// <param name="imageURL"></param>
        /// <returns></returns>
        public bool Update(Book book, string imageURL, int Quantity)
        {
            try
            {
                var bookOld = db.Books.Find(book.BookID);
                bookOld.ImageBool.Images.First().ImageURL = imageURL;
                bookOld.TotalQuantity += Quantity;
                bookOld.CurrentQuantity += Quantity;
                //string updateBook = "update Books set CurrentQuantity = " + bookOld.CurrentQuantity + Quantity + ",TotalQuantity = " + bookOld.TotalQuantity + Quantity + " where BookID =  "+ book.BookID +")";
                //db.Database.ExecuteSqlCommand(updateBook);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        // <summary>
        /// Thêm 1 cuốn sách
        /// </summary>
        /// <param name="book"></param>
        /// <param name="imageURL"></param>
        /// <returns>trả về ID của cuốn sách được thêm</returns>
        public int Insert(Book book, string imageURL , string Authors, string Category)
        {
            //db.Books.Add(book);
            var imageBool = new ImageBool();
            var image = db.ImageBools.Add(imageBool);
            //string qrInsertImage = "insert into Images values (" + image.ImageBoolID + ", N'" + imageURL + "')";
            //db.Database.ExecuteSqlCommand(qrInsertImage);
            string InsertBook = "insert into Books values (N'" + book.Name + "'," + book.PublisherID + ",N'" + book.PublicationDate + "',N'1',N'" + book.Overview + "',N'" + book.Details + "',N'" + book.Price + "',N'" + book.TotalQuantity + "',N'" + book.ViewCount + "',0,N'" + book.TotalQuantity + "')";
            db.Database.ExecuteSqlCommand(InsertBook);
            db.SaveChanges();
            string qrAuthorBook = "insert into AuthorsBooks (AuthorID, BookID) values (" + Authors + ",  (SELECT IDENT_CURRENT('Books') as LastID))";
            db.Database.ExecuteSqlCommand(qrAuthorBook);
            string qrCategoryBook = "insert into CategoriesBooks (BookID, CategoryID) values ( (SELECT IDENT_CURRENT('Books') as LastID) , " + Category + ")";
            db.Database.ExecuteSqlCommand(qrCategoryBook);
            db.SaveChanges();

            string qrInsertImage = "insert into Images values (" + image.ImageBoolID + ", N'" + imageURL + "');";
            string updateBookImage = "update Books set ImageBoolID = " + image.ImageBoolID + " where BookID =  (SELECT IDENT_CURRENT('Books') as LastID)";
            db.Database.ExecuteSqlCommand(qrInsertImage);
            db.Database.ExecuteSqlCommand(updateBookImage);

            db.SaveChanges();
            return book.BookID;
        }

        /// <summary>
        /// Lấy danh sách Book theo phan trang
        /// </summary>
        /// <param name="searchString"
        /// <param name="page"
        /// <param name="pageSize"
        ///<return></return>
        public IEnumerable<Book> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Book> model = db.Books;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = db.Books.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderBy(x => x.BookID).ToPagedList(page, pageSize);
        }
        public bool Delete(int id)
        {
            try
            {
                var book = db.Books.Find(id);
                db.Books.Remove(book);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        // <summary>
        /// Thêm 1 cuốn sách
        /// </summary>
        /// <param name="book"></param>
        /// <param name="imageURL"></param>
        /// <returns>trả về ID của cuốn sách được thêm</returns>
        public bool UpdateCurrent(int bookID ,int quality, int current)
        {
            try
            {
                var bookOld = db.Books.Find(bookID);
                bookOld.CurrentQuantity = quality - current;

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

    }
}