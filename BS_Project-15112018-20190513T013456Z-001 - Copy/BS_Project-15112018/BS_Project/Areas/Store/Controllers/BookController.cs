using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using PagedList;
using BS_Project.EF;
using BS_Project.DAO;
using System.Collections.Generic;
using System;
using BS_Project.Areas.Store.Models;

namespace BS_Project.Areas.Store.Controllers
{
    public class BookController : BaseController
    {
        // GET: Store/Book
        private BSDBContext db = new BSDBContext();

        /// <summary>
        /// Hiện thị thông tin của tất cả cuốn sách trên trang index
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index(string searchString, int page = 1, int pageSize = 3)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            IQueryable<Book> books = db.Books.Include(b => b.ImageBool).Include(b => b.Publisher);
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(x => x.Name.Contains(searchString) || x.Publisher.Name.Contains(searchString) || x.Authors.FirstOrDefault().Name.Contains(searchString));
            }
            ViewBag.SearchString = searchString;
            return View(books.OrderBy(x => x.BookID).ToPagedList(page, pageSize));

            //var dao = new BookDAO();
            //var model = dao.ListAllPaging(searchString, page, pageSize);
            //ViewBag.SearchString = searchString;
            //return View(model);
        }

        /// <summary>
        /// Hiện thị trang sửa 1 cuốn sách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageBoolID = new SelectList(db.ImageBools, "ImageBoolID", "ImageBoolID", book.ImageBoolID);
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", book.Authors.First().AuthorID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name", book.PublisherID);
            return View(book);
        }

        /// <summary>
        /// Sửa 1 cuốn sách và lưu nó vào database
        /// </summary>
        /// <param name="book"></param>
        /// <param name="formcollection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book, FormCollection formcollection)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            string imageURL, totalQuantity = null;
            try
            {
                imageURL = formcollection["txtImageURL"].ToString();
            }
            catch
            {
                imageURL = "/Content/images/Image.jpg";
            }
            totalQuantity = formcollection["CurrentQuantity"].ToString();
            if (ModelState.IsValid)
            {
                var dao = new BookDAO();
                var rs = dao.Update(book, imageURL, Int32.Parse(totalQuantity));
                return RedirectToAction("Index");
            }
            ViewBag.ImageBoolID = new SelectList(db.ImageBools, "ImageBoolID", "ImageBoolID", book.ImageBoolID);
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", book.Authors.First().AuthorID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name", book.PublisherID);
            return View(book);
        }

        /// <summary>
        /// Hiện thị thông tin chi tiết của 1 cuốn sách chọn theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        /// <summary>
        /// hiện thị trang tạo mới 1 cuốn sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            ViewBag.ImageBoolID = new SelectList(db.ImageBools, "ImageBoolID", "ImageBoolID");
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name");
            return View();
        }

        /// <summary>
        /// Tạo mới 1 cuốn sách và lưu nó lại
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Book book, FormCollection formcollection/*, Author author, Category category*/)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            string imageURL, a, c = null;
            try
            {
                imageURL = formcollection["txtImageURL"].ToString();
            }
            catch
            {
                imageURL = "/Content/images/Image.jpg";
            }
            //a = Int32.Parse(formcollection["AuthorID"].ToString());
            //c = Int32.Parse(formcollection["CategoryID"].ToString());
            //var auth = new AuthorDAO();
            //var cete = new CategoryDAO();
            //book.Authors = new List<Author>();
            //book.Authors.Add(auth.ForAuthBook(a));
            //book.Categories = new List<Category>();
            //book.Categories.Add(cete.ForCateBook(c));
            //book.Authors = new List<Author>();
            //book.Authors.Add(author);
            //book.Categories = new List<Category>();
            //book.Categories.Add(category);
            a = formcollection["AuthorID"].ToString();
            c = formcollection["CategoryID"].ToString();
            if (ModelState.IsValid)
            {
                var dao = new BookDAO();
                var rs = dao.Insert(book, imageURL, a ,c);  // bay gio nếu insert book thì author đi cùng tự động insert luôn. Làm tương tự cho trường hợp thêm Author
                await db.SaveChangesAsync();
                SetAlert("Thêm Sách Thành Công", "success");
                return RedirectToAction("Index");
            }
            ViewBag.ImageBoolID = new SelectList(db.ImageBools, "ImageBoolID", "ImageBoolID", book.ImageBoolID);
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", book.Authors.First().AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name", book.PublisherID);
            return View(book);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var dao = new BookDAO();
            var result = dao.Delete(id);
            if (result)
            {
                return RedirectToAction("Index", "Book");
            }
            else
            {
                ModelState.AddModelError("", "Xóa không thành công");
            }
            return View("Index");
        }
        public JsonResult DeleteByID(int id)
        {
            var result = new BookDAO().Delete(id);
            if (result)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }

        }
    }
}