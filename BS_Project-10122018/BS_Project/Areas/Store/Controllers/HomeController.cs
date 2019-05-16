using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Areas.Store.Controllers
{
    public class HomeController : Controller
    {
        // GET: Store/Home
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            return View("~/Areas/Store/Views/Home/Index.cshtml");
        }
    }
}