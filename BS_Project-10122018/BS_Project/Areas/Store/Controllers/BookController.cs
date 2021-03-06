﻿using System.Data;
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
            //IQueryable<Book> books = db.Books.Include(b => b.ImageBool).Include(b => b.Publisher).Where(x => x.isDeleted == false);
            IQueryable<Book> books = db.Books.Include(b => b.Publisher).Where(x => x.isDeleted == false);
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(x => x.Name.Contains(searchString) || x.Publisher.Name.Contains(searchString) || x.Authors.FirstOrDefault().Name.Contains(searchString));
            }
            ViewBag.SearchString = searchString;
            return View("~/Areas/Store/Views/Book/Index.cshtml", books.OrderBy(x => x.BookID).ToPagedList(page, pageSize));
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
            ViewBag.ImageBoolID = new SelectList(db.Images, "ImageBoolID", "ImageBoolID", book.ImageBoolID);
            ViewBag.AuthorID = new SelectList(db.Authors.Where(x => x.isDeleted == false), "AuthorID", "Name", book.Authors.First().AuthorID);
            ViewBag.PublisherID = new SelectList(db.Publishers.Where(x => x.isDeleted == false), "PublisherID", "Name", book.PublisherID);
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x => x.isDeleted == false), "CategoryID", "Name", book.Categories.First().CategoryID);
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
            string imageURL = null;
            try
            {
                imageURL = formcollection["txtImageURL"].ToString();
            }
            catch
            {
                imageURL = "/Content/images/Image.jpg";
            }
            if (ModelState.IsValid)
            {
                var dao = new BookDAO();
                var rs = dao.Update(book, imageURL);
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImageBoolID = new SelectList(db.Images, "ImageBoolID", "ImageBoolID", book.ImageBoolID);
            //ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", book.Authors.First().AuthorID);
            //ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name", book.PublisherID);
            ViewBag.AuthorID = new SelectList(db.Authors.Where(x => x.isDeleted == false), "AuthorID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x => x.isDeleted == false), "CategoryID", "Name");
            ViewBag.PublisherID = new SelectList(db.Publishers.Where(x => x.isDeleted == false), "PublisherID", "Name");
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
            ViewBag.ImageBoolID = new SelectList(db.Images, "ImageBoolID", "ImageBoolID");
            ViewBag.AuthorID = new SelectList(db.Authors.Where(x => x.isDeleted == false), "AuthorID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories.Where(x => x.isDeleted == false), "CategoryID", "Name");
            ViewBag.PublisherID = new SelectList(db.Publishers.Where(x => x.isDeleted == false), "PublisherID", "Name");
            return View();
        }

        /// <summary>
        /// Tạo mới 1 cuốn sách và lưu nó lại
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Book book, FormCollection formcollection, Author author, Category category)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            string imageURL = null;
            try
            {
                imageURL = formcollection["txtImageURL"].ToString();
            }
            catch
            {
                imageURL = "/Content/images/Image.jpg";
            }

            if (ModelState.IsValid)
            {
                var dao = new BookDAO();
                book.isDeleted = false;
                var rs = dao.Insert(book, imageURL);  // bay gio nếu insert book thì author đi cùng tự động insert luôn. Làm tương tự cho trường hợp thêm Author
                if(rs != -1)
                {
                    new AuthorDAO().InsertAuthorsBooks(rs, author.AuthorID);
                    new CategoryDAO().InsertCategorysBooks(rs, category.CategoryID);
                }
                await db.SaveChangesAsync();
                SetAlert("Thêm Sách Thành Công", "success");
                return RedirectToAction("Index");
            }
            ViewBag.ImageBoolID = new SelectList(db.Images, "ImageBoolID", "ImageBoolID", book.ImageBoolID);
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", book.Authors.First().AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name", book.PublisherID);
            return View(book);
        }

        [HttpPost]
        public ActionResult DeleteBook(int bookID)
        {
            int result = 1;
            var dao = db.Books.Find(bookID);
            dao.isDeleted = true;
            try
            {
                db.SaveChanges();
            }catch(Exception e)
            {
                result = -1;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}