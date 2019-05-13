using BS_Project.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Areas.Store.Controllers
{
    public class CommentController : Controller
    {
        // GET: Store/Comment
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new CommentsDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
    }
}