using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BS_Project.Models;
using System.Web.Script.Serialization;
using BS_Project.DAO;
using System.Threading.Tasks;
using BS_Project.EF;
using System.Configuration;
using Common;

namespace BS_Project.Controllers
{
    public class OrderBookController : Controller
    {
        // GET: OrderBook
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Payment()
        {
            if (Session["userID"] == null)
            {
                return RedirectToAction("/");
            }
            var cart = Session[CommonConstant.cartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            int userId = Convert.ToInt32(Session["userID"]);
            UserDAO userDAO = new UserDAO();
            User user = userDAO.GetUserById(userId);
            if(string.IsNullOrEmpty(user.Phone) || string.IsNullOrEmpty(user.Address) || string.IsNullOrEmpty(user.Email))
            {
                return RedirectToAction("EditProfile", "ProfileUser", new { user.Username, returnUrl = Url.Action("Payment","OrderBook") });
            }
            return View(list);
        }

        /// <summary>
        /// Khi thanh toán thì lưu vào OrderBook và OrderDetail. Sau đó gửi email cho Khách hàng và chạy đến trang thanh toán Paypal/ Ngân lượng.
        /// </summary>
        /// <param name="CustName">Tên Khách Hàng</param>
        /// <param name="CustPhone">Số điện thoại khách hàng</param>
        /// <param name="CustAdd">Địa chỉ khách hàng</param>
        /// <param name="CustEmail">Email Khách Hàng</param>
        /// <param name="thanhtoantructuyenNL">Check Thanh toán Ngân lượng</param>
        /// <param name="thanhtoantructuyenPP">Check Thanh toán Paypal</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Payment(string CustName, string CustPhone, string CustAdd, string CustEmail, bool? thanhtoantructuyenNL, bool? thanhtoantructuyenPP)
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("LoginUsernormal", "LoginUser");
            }
            var orderBook = new OrdersBook();
            int userID = Convert.ToInt32(Session["userID"]);
            //Lưu vào trong db OrderBook
            orderBook.FoundedDate = DateTime.Now;
            orderBook.UserID = userID;
            orderBook.Status = 0; 
            orderBook.Address = CustAdd;
            orderBook.Phone = CustPhone;
            orderBook.Email = CustEmail;
            orderBook.FullName = CustName;
            orderBook.Paid = false; // Mặc định là chưa thanh toán
            bool result = new OrderBookDAO().Insert(orderBook);
            try
            {
                if (result)
                {
                    int tempID = orderBook.OrderID;

                    var cart = (List<CartItem>)Session[CommonConstant.cartSession];
                    var detailDAO = new OrdersDetailsDAO();
                    double sum = 0;
                    foreach (var item in cart)
                    {
                        var orderDetail = new OrdersDetail();
                        orderDetail.OrderID = tempID;
                        orderDetail.BookID = item.Books.BookID;
                        orderDetail.Total = (item.Quantity * item.Books.Price);
                        orderDetail.Quantity = item.Quantity;
                        orderDetail.Status = 0;
                        detailDAO.Insert(orderDetail);

                        sum += (item.Books.Price.GetValueOrDefault(0) * item.Quantity);
                    }

                    //Gửi mail thông báo đơn hàng cho Khách hàng
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Store/Views/template/newOrder.cshtml"));
                    content = content.Replace("{{CustomerName}}", CustName);
                    content = content.Replace("{{Phone}}", CustPhone);
                    content = content.Replace("{{Email}}", CustEmail);
                    content = content.Replace("{{Address}}", CustAdd);
                    content = content.Replace("{{Total}}", sum.ToString("N0"));
                    new MailHelper().SendMail(CustEmail, "Đơn hàng mới từ QT BookStore", content, "Thông báo Đơn Đặt hàng từ QT BookStore");

                    if ((thanhtoantructuyenNL == null || thanhtoantructuyenNL == false) && (thanhtoantructuyenPP == null || thanhtoantructuyenPP == false))
                    {
                        Session[CommonConstant.cartSession] = null;
                        return Redirect("/hoan-thanh");
                    }
                    else if (thanhtoantructuyenPP != null || thanhtoantructuyenPP == true)
                    {
                        var model = new CommonConstant.InforPaypal
                        {
                            OrderId = tempID,
                            Total = cart.Sum(m => m.Quantity * m.Books.Price) ?? 0
                        };
                        TempData["InforPaypal"] = model;
                        return RedirectToAction("PaymentWithPaypal", "Paypal");
                    }
                    else
                    {
                        NL_Checkout nganluong = new NL_Checkout();
                        double total = cart.Sum(m => m.Quantity * m.Books.Price) ?? 0;
                        string urlThanhToan = nganluong.buildCheckoutUrlNew(Url.Action("PaymentConfirmed", "OrderBook", null, Request.Url.Scheme), ConfigurationManager.AppSettings["email_nganluong"].ToString() , "Thanh toán " + total.ToString("#,##0").Replace(',', '.') + " đồng", tempID.ToString(), total.ToString(), "vnd", 1, 0, 0, 0, 0, "Thanh toán " + total.ToString("#,##0").Replace(',', '.') + " đồng",
                         "Thanh toán " + total.ToString("#,##0").Replace(',', '.') + " đồng", "");
                        return Redirect(urlThanhToan);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Không thể thêm đơn hàng vì xảy ra lỗi ở server!");
                    return View("ThongBaoLoi");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("ThongBaoLoi");
            }
        }

        /// <summary>
        /// Chuyển trạng thái đơn hàng đã thanh toán
        /// Trừ số lượng sách KDH vừa mua
        /// </summary>
        /// <param name="orderID"></param>
        public void ConvertPaidToTrue(int orderID)
        {
            BSDBContext db = new BSDBContext();
            var model = db.OrdersBooks.Find(orderID);
            model.Paid = true;
            db.SaveChanges();
            var detail = db.OrdersDetails.Where(x => x.OrderID == orderID).ToList();
            foreach (var item in detail)
            {
                var book = db.Books.Find(item.BookID);
                int temp = book.TotalQuantity - item.Quantity.GetValueOrDefault();
                if (temp <= 0)
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

        // ok roi. 
        public async Task<ActionResult> PaymentConfirmed(string transaction_info, string order_code, int price, string payment_id, string payment_type, string error_text, string secure_code)
        {
            BSDBContext db = new BSDBContext();
            if (error_text == "")
            {
                int Id = int.Parse(order_code);
                OrdersBook Order = db.OrdersBooks.Find(Id);
                Order.Paid = true; // Đã thanh toán
                db.Entry(Order).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                var orderBook = new OrderBookDAO().Get(Id);
                historyBankCharging history = new historyBankCharging()
                {
                    email = orderBook.Email,
                    phone = orderBook.Phone,
                    fullname = orderBook.FullName,
                    date_trans = DateTime.Now,
                    price = price,
                    order_code = order_code,
                    error_text = error_text,
                    transaction_info = transaction_info,
                    payment_id = payment_id,
                    payment_type = payment_type,
                    secure_code = secure_code
                };
                db.historyBankChargings.Add(history);
                await db.SaveChangesAsync();

                // giam so luong ton cua cac sách khách đã thanh toán
                foreach (OrdersDetail ordersDetail in Order.OrdersDetails)
                {
                    Book book = db.Books.Find(ordersDetail.BookID);
                    book.TotalQuantity -= (ordersDetail.Quantity ?? 0); // giảm 
                    db.Entry(book).State = System.Data.Entity.EntityState.Modified;  
                    await db.SaveChangesAsync();
                }
                
                Session[CommonConstant.cartSession] = null;
                return Redirect("/hoan-thanh");
            }
            else
                return View("ThongBaoLoi");
        }

        public async Task<ActionResult> PaypalConfirmed()
        {
            try
            {
                BSDBContext db = new BSDBContext();
                var model = (CommonConstant.InforPaypal)TempData["InforOrder"];
                int Id = model.OrderId;
                OrdersBook Order = db.OrdersBooks.Find(Id);
                Order.Paid = true; // Đã thanh toán
                db.Entry(Order).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                var orderBook = new OrderBookDAO().Get(Id);
                historyBankCharging history = new historyBankCharging()
                {
                    email = orderBook.Email,
                    phone = orderBook.Phone,
                    fullname = orderBook.FullName,
                    date_trans = DateTime.Now,
                    price = (int)model.Total,
                    order_code = null,
                    error_text = null,
                    transaction_info = null,
                    payment_id = null,
                    payment_type = "Paypal",
                    secure_code = null
                };
                db.historyBankChargings.Add(history);
                await db.SaveChangesAsync();

                // giam so luong ton cua cac sách khách đã thanh toán
                foreach (OrdersDetail ordersDetail in Order.OrdersDetails)
                {
                    Book book = db.Books.Find(ordersDetail.BookID);
                    book.TotalQuantity -= (ordersDetail.Quantity ?? 0); // giảm 
                    db.Entry(book).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                Session[CommonConstant.cartSession] = null;
                return Redirect("/hoan-thanh");
            }
            catch(Exception ex)
            {
                return View("ThongBaoLoi");
            }
        }

        public ActionResult Succeed()
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("LoginUsernormal", "LoginUser");
            }
            return View();
        }

    }
}