using BS_Project.DAO;
using BS_Project.EF;
using BS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Areas.Store.Controllers
{
    public class OrderController : Controller
    {
        private BSDBContext db = new BSDBContext();
        // GET: Store/Order
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View("~/Areas/Store/Views/Order/Index.cshtml", model);
        }

        [HttpPost]
        public ActionResult TransferStatus(int orderID)
        {
            var dao = db.OrdersBooks.Find(orderID);
            if(dao.Status == 3)
            {
                dao.Status++;
                dao.Paid = true;
                var detail = db.OrdersDetails.Where(x => x.OrderID == orderID).ToList();
                foreach(var item in detail)
                {
                    var book = db.Books.Find(item.BookID);
                    int temp = book.TotalQuantity - item.Quantity.GetValueOrDefault();
                    if(temp <= 0)
                    {
                        book.TotalQuantity = 0;
                        db.SaveChanges();
                    }
                    else
                    {
                        book.TotalQuantity = temp;
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                dao.Status++;
            }
            int result = 1;
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                result = -1;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CountCanceled(int userID)
        {
            var model = db.OrdersBooks.Where( x => x.UserID == userID && x.Canceled == true).Count();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ShowOrderByStatusOne(string search_value, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(0, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            return PartialView("_ShowOrderByStatusOne", model);
        }

        [HttpGet]
        public ActionResult ShowOrderByStatusOne_Shadow(string search_value, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(0, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            ViewBag.Check = 1;
            ViewBag.Layout = "~/Areas/Store/Views/Shared/_Layout.cshtml";
            return PartialView("_ShowOrderByStatusOne", model);
        }
        
        [HttpGet]
        public ActionResult ShowOrderByStatusTwo(string search_value, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(1, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            return PartialView("_ShowOrderByStatusTwo", model);
        }

        [HttpGet]
        public ActionResult ShowOrderByStatusTwo_Shadow(string search_value, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(1, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            ViewBag.Check = 1;
            ViewBag.Layout = "~/Areas/Store/Views/Shared/_Layout.cshtml";
            return PartialView("_ShowOrderByStatusTwo", model);
        }

        [HttpGet]
        public ActionResult ShowOrderByStatusThree(string search_value, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(2, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            return PartialView("_ShowOrderByStatusThree", model);
        }

        [HttpGet]
        public ActionResult ShowOrderByStatusThree_Shadow(string search_value, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(2, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            ViewBag.Check = 1;
            ViewBag.Layout = "~/Areas/Store/Views/Shared/_Layout.cshtml";
            return PartialView("_ShowOrderByStatusThree", model);
        }

        [HttpGet]
        public ActionResult ShowOrderByStatusFour(string search_value, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(3, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            return PartialView("_ShowOrderByStatusFour", model);
        }

        [HttpGet]
        public ActionResult ShowOrderByStatusFour_Shadow(string search_value, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(3, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            ViewBag.Check = 1;
            ViewBag.Layout = "~/Areas/Store/Views/Shared/_Layout.cshtml";
            return PartialView("_ShowOrderByStatusFour", model);
        }

        [HttpGet]
        public ActionResult ShowOrderByStatusFive(string search_value, int page = 1, int pageSize = 5) {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(4, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            return PartialView("_ShowOrderByStatusFive", model);
        }

        [HttpGet]
        public ActionResult ShowOrderByStatusFive_Shadow(string search_value, int page = 1, int pageSize = 5)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new OrderBookDAO();
            var model = dao.ListAllPagingFilter(4, search_value, page, pageSize);
            ViewBag.SearchString = search_value;
            ViewBag.Check = 1;
            ViewBag.Layout = "~/Areas/Store/Views/Shared/_Layout.cshtml";
            return PartialView("_ShowOrderByStatusFive", model);
        }
    }
}