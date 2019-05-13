using BS_Project.DAO;
using BS_Project.EF;
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
            var dao = new OrderBookDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpPost]
        public ActionResult TransferStatus(int orderID)
        {
            var dao = db.OrdersBooks.Find(orderID);
            dao.Status++;
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
    }
}