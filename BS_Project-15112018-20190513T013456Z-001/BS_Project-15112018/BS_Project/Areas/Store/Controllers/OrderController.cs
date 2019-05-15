using BS_Project.Areas.Store.Models;
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
                if (dao.Status == 4)
                {
                    var bookDetail = from table in db.OrdersDetails
                               where table.OrderID == orderID
                               select table;

                    foreach (var bookD in bookDetail)
                    {
                        var bookS = from table in db.Books
                                   where table.BookID == bookD.BookID
                                    select table;
                        foreach (var book in bookS)
                        {
                            var daoBook = new BookDAO();
                            daoBook.UpdateCurrent(book.BookID, Int32.Parse(book.CurrentQuantity.ToString()), Int32.Parse(bookD.Quantity.ToString()));
                        }
                    }
                    dao.Paid = true;
                }
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