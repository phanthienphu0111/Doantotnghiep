using BS_Project.DAO;
using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Areas.Store.Controllers
{
    public class CategoryController : Controller
    {
        private BSDBContext db = new BSDBContext();
        // GET: Store/Category
        /// <summary>
        /// Action Index sẽ đưa tới danh sách Catogory có trong CSDL
        /// </summary>
        /// <param name="searchString">Chuỗi tìm kiếm</param>
        /// <param name="page">Trang mặc định khi người dùng load vào</param>
        /// <param name="pageSize">Số record trong một trang</param>
        /// <returns></returns>
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new CategoryDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        /// <summary>
        /// Action đưa tới View tạo mới Category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Post dữ liêu nhập vào từ View Index của Category đến Action Create HttpPost để thực hiện chức năng Insert
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var dao = new CategoryDAO();
                int id = dao.Insert(category);
                if (category.CategoryID > 0)
                {

                    return RedirectToAction("Index", "Category");
                }
                else
                {

                    ModelState.AddModelError("", "Thêm Category không thành công");
                }
            }

            return View("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JsonResult CheckNameInput(string name)
        {
            bool cateDetail = new CategoryDAO().CheckName(name);
            if (cateDetail)
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
        /// <summary>
        /// Gọi đên View Edit của Category với CategoryID = id
        /// </summary>
        /// <param name="id">CategoryID</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var categoryDetail = new CategoryDAO().ViewDetail(id);
            return View(categoryDetail);
        }

        /// <summary>
        /// Post dữ liệu đã Edit ở View Edit Category băng phương thức HttpPost để thực hiện chức năng Edit
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var dao = new CategoryDAO();


                var result = dao.Update(category);

                if (result)
                {

                    return RedirectToAction("Index", "Category");
                }
                else
                {

                    ModelState.AddModelError("", "Cập nhật không thành công");
                }

            }

            return View("Index");
        }

        /// <summary>
        /// Thực hiện chức năng xóa record với CategoryID = id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var dao = new CategoryDAO();
            var result = dao.Delete(id);
            if (result)
            {
                return RedirectToAction("Index", "Category");
            }
            else
            {
                ModelState.AddModelError("", "Xóa không thành công");
            }
            return View("Index");
        }

        public JsonResult ListName(string q)
        {
            var data = new CategoryDAO().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAuthor(string q)
        {
            var author = db.Authors.Where(x => x.Name.Contains(q)).Select(x => x.Name).ToList();

            return Json(author, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteByID(int id)
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
    }
}