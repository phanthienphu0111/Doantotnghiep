using BS_Project.DAO;
using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Areas.Store.Controllers
{
    public class AuthorController : Controller
    {
        private BSDBContext db = new BSDBContext();

        // GET: Store/Author

        /// <summary>
        /// phân trang cho trang index có 5 dòng
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// PartialViewResult hiện thị tất cả cá tác giả
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PartialViewResult returnTableAuthor(string searchString, int page = 1, int pageSize = 4)
        {
            var dao = new AuthorDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return PartialView(model);
        }

        /// <summary>
        /// Thanh search có autocomplete tên tác giả
        /// </summary>
        /// <returns></returns>
        public PartialViewResult completeNameAuthor()
        {
            return PartialView();
        }

        /// <summary>
        /// PartialViewResult drop down list tác giả
        /// </summary>
        /// <returns></returns>
        public PartialViewResult returnDropDownList()
        {
            var model = db.Authors.ToList();
            ViewBag.Name = new SelectList(db.Authors.Select(x => new { x.Name, x.AuthorID }));
            //ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name");
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Tạo thêm 1 tác giả mới
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Author author, FormCollection formcollection)
        {
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
                var dao = new AuthorDAO();
                int result = dao.Insert(author, imageURL);
                if (result > 0)
                {
                    return RedirectToAction("Index", "Author");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm tác giả không thành công");
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Sửa 1 tác giả
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var authorDetail = new AuthorDAO().ViewDetail(id);
            return View(authorDetail);
        }

        /// <summary>
        /// Sửa 1 tác giả
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Author author, FormCollection formcollection)
        {
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
                var dao = new AuthorDAO();

                var result = dao.Update(author, imageURL);
                if (result)
                {
                    return RedirectToAction("Index", "Author");
                }
                else
                {
                    ModelState.AddModelError("", "tác giả không cập nhật thành công");
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Xoá đi 1 tác giả
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            var result = new AuthorDAO().Delete(id);
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

        [HttpPost]
        public JsonResult GetBook(string q)
        {
            var author = db.Authors.Where(x => x.Name.Contains(q)).Select(x => x.Name).ToList();

            return Json(author);
        }

        public string ViewString(int id)
        {
            var dao = new AuthorDAO().ViewDetail(id);
            string Overview = "";
            if (dao.Overview == null)
            {
                Overview = "";
            }
            else
            {
                Overview = dao.Overview.ToString();
            }
            string ovv = "";
            if (Overview.Length > 100)
            {
                ovv = Overview.Substring(0, 100) + "...";
            }
            else
                ovv = Overview;
            return ovv;
        }
    }
}