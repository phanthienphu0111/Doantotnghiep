using BS_Project.DAO;
using BS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BS_Project.EF;

namespace BS_Project.Controllers
{
    public class CartItemController : Controller
    {
        // GET: CartItem
        public ActionResult Index()
        {
            var list = Session[CommonConstant.cartSession] as List<CartItem>;
            return View(list);
        }

        /// <summary>
        /// Thêm book vao giỏ hàng
        /// </summary>
        /// <param name="bookID"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public JsonResult AddItem(int bookID, int quantity)
        {
            var book = new BookDAO().BookDetail(bookID);
            var status = true;
            if (Session[CommonConstant.cartSession] == null)
                Session[CommonConstant.cartSession] = new List<CartItem>();

            List<CartItem> list = Session[CommonConstant.cartSession] as List<CartItem>;
            //đơn hàng đang được giao tới...
            int orderShipping = 0;
            if (Session["userID"] != null) {
                int userID = (Int32)Session["userID"];
                CartItem cartEdit = list.SingleOrDefault(m => m.Books.BookID == bookID);
                int totalBuy = list.Sum(m=>m.Quantity);
                
                if(cartEdit != null && cartEdit.Quantity == book.TotalQuantity)
                {
                    //Xuất ra thông báo số lượng sách bạn đã mua vượt quá 10
                    return Json(new
                    {
                        totalBuy = 0,
                        Status = false,
                        ErrorMessage = "Bạn đã chọn vượt quá số lượng tồn"
                    });
                }

                if (totalBuy < 10) {
                    //Sửa số lượng của item trong giỏ hàng
                    if (cartEdit != null)
                        cartEdit.Quantity = quantity;
                    else {
                        //Tạo mới giỏ hàng
                        var item = new CartItem();
                        item.Books = book;
                        item.Quantity = quantity;
                        //Thêm sản phẩm vào giỏ hàng
                        list.Add(item);
                    }
                    Session[CommonConstant.cartSession] = list;
                    return Json(new
                    {
                        totalBuy = list.Sum(m => m.Quantity * m.Books.Price),
                        Status = status

                    });

                }
                else {
                    //Xuất ra thông báo số lượng sách bạn đã mua vượt quá 10
                    return Json(new
                    {
                        totalBuy = 0,
                        Status = false,
                        ErrorMessage = "Bạn đã mua vượt quá 10"
                    });
                }
            }
            else {
                //Xuất ra thông báo chưa đăng nhập
                return Json(new
                {
                    totalBuy = 0,
                    Status = false,
                    ErrorMessage = "Bạn chưa đăng nhập"
                });
            }
        }

        /// <summary>
        /// Action Update Order Cart sẽ tiến hành Update giỏ hàng được gọi lên từ cartController
        /// </summary>
        /// <param name="cartModel">List Book cần Update</param>
        /// <returns></returns>
        public JsonResult Update(string cartModel)
        {
            BSDBContext db = new BSDBContext();

            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CommonConstant.cartSession];
            if(jsonCart.Sum(p=>p.Quantity) > 10)
            {
                return Json(new
                {
                    status = false,
                    ErrorMessage = "Tổng số lượng không thể quá 10 sách"
                });
            }
            foreach (var item in sessionCart) {
                Book book = db.Books.Find(item.Books.BookID);
                var jsonItem = jsonCart.SingleOrDefault(x => x.Books.BookID == item.Books.BookID);
                if (jsonItem != null) {
                    if(jsonItem.Quantity <= book.TotalQuantity )
                        item.Quantity = jsonItem.Quantity;
                }
            }

            Session[CommonConstant.cartSession] = sessionCart;
            return Json(new {
                status = true
            });
        }

        /// <summary>
        /// Xóa tất cả giỏ hàng
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteAll()
        {
            Session[CommonConstant.cartSession] = null;
            return Json(new {
                status = true
            });
        }

        // <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteByID(int id)
        {
            var sessionCart = (List<CartItem>)Session[CommonConstant.cartSession];
            sessionCart.RemoveAll(x => x.Books.BookID == id);
            Session[CommonConstant.cartSession] = sessionCart;
            return Json(new {
                status = true
            });
        }


    }
}