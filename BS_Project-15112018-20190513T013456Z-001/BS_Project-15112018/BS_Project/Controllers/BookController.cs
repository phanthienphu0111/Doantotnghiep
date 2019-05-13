using BS_Project.EF;
using BS_Project.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace BS_Project.Controllers
{
    public class BookController : Controller
    {
        BSDBContext db = new BSDBContext();
        RemoveSignVNModel removeSign = new RemoveSignVNModel();
        CommonConstant cm = new CommonConstant();
        private static int pageSize = 6;
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ShowBook()
        {
            return View();
        }

        /// <summary>
        /// hiển thị tất cả các sách
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sortID"></param>
        /// <returns>trả về danh sách tất cả các book</returns>
        [HttpGet]
        public ActionResult ShowAllBook(int page = 0, int sortID = 0)
        {
            //hiện tại dữ liệu còn ít, nên test 1 trang có 3 sản phẩm để làm phân trang
            // tương lai sẽ cho người dùng chọn pageSize
            if (Request.QueryString["page"] != null)
            {
                page = int.Parse(Request.QueryString["page"].ToString());
            }

            //gọi procedure ShowBookFollowCategory
            var allBooks = db.Database.SqlQuery<Book>("ShowAllBook @sortID",
                new SqlParameter("@sortID", sortID)).ToList();

            // số book hiển thị
            var books = allBooks.Skip(page * 3).Take(pageSize).ToList();
            var totalPage = (int)Math.Ceiling((decimal)allBooks.Count / pageSize);

            return PartialView("~/Views/Shared/_ShowAllBook.cshtml", new PaggingModel
            {
                Books = books,
                TotalPage = totalPage
            });
        }

        /// <summary>
        /// Show thể loại của các loại sách
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowCategoriesBook()
        {
            var allCategories = db.Categories.ToList();
            var totalCate = allCategories.Count;
            return PartialView("~/Views/Shared/_ShowCategoriesBook.cshtml", new PaggingModel
            {
                ToTalCategories = totalCate,
                Categories = allCategories
            });
        }

        /// <summary>
        /// đếm số lượng sách của từng thể loại
        /// </summary>
        /// <param name="idCate">id thể loại</param>
        /// <returns>số sách theo thể loại</returns>
        public int GetCountForCategory(int idCate)
        {
            return db.Categories.Find(idCate).Books.Count();
        }

        /// <summary>
        /// hàm lấy ảnh đầu tiên của quyển sách để hiển thị 
        /// </summary>
        /// <param name="idImage">idpoolimage của từng sách</param>
        /// <returns>trả về chuỗi string chứa hình</returns>
        public string GetFirstImage(int idImage)
        {
            string image = "";
            image = db.Images.Where(x => x.ImageBoolID == idImage).First().ImageURL;
            return image;
        }

        /// <summary>
        /// định dạng tiền vnđ
        /// </summary>
        /// <param name="money">chuỗi số cần format tiền</param>
        /// <returns></returns>
        public string FormatMoney(string money)
        {
            return cm.formatMoney(money, ".");
        }

        /// <summary>
        /// Hiển thị sách theo nhà phát hành
        /// </summary>
        /// <param name="page">page hiện tại</param>
        /// <param name="idPublisher">id của loại sách</param>
        /// <param name="sortID">sortid = 1 theo sách xem nhiều, = 2 sắp xếp theo ngày phát hành, = 3 sách được đặt nhiều</param>
        /// <returns>trả về 1 partialview, kèm theo 1 model books truyền qua</returns>
        [HttpGet]
        public ActionResult ShowBookFollowPublisher(int page = 0, int idPublisher = 0, int sortID = 0)
        {
            //hiện tại dữ liệu còn ít, nên test 1 trang có 3 sản phẩm để làm phân trang
            //tương lai sẽ cho người dùng chọn pageSize
            if (Request.QueryString["idPublisher"] != null)
            {
                idPublisher = int.Parse(Request.QueryString["idPublisher"].ToString());
            }
            if (Request.QueryString["page"] != null)
            {
                page = int.Parse(Request.QueryString["page"].ToString());
            }

            //gọi procedure ShowBookFollowCategory
            var allBooks = db.Database.SqlQuery<Book>("ShowBookFollowPublisher @publicsherID, @sortID",
                new SqlParameter("@publicsherID", idPublisher), new SqlParameter("@sortID", sortID)).ToList();

            // số book hiển thị
            var books = allBooks.Skip(page * 3).Take(pageSize).ToList();
            var totalPage = (int)Math.Ceiling((decimal)allBooks.Count / pageSize);
            return PartialView("~/Views/Shared/_ShowBookFollowPublisher.cshtml", new PaggingModel
            {
                Books = books,
                TotalPage = totalPage,
                idPublisher = idPublisher
            });
        }

        /// <summary>
        /// Hiển thị sách theo thể loại sách
        /// </summary>
        /// <param name="page">page hiện tại</param>
        /// <param name="idCategory">id của loại sách</param>
        /// <param name="sortID">sortid = 1 theo sách xem nhiều, = 2 sắp xếp theo ngày phát hành, = 3 sách được đặt nhiều</param>
        /// <returns>trả về 1 partialview, kèm theo 1 model books truyền qua</returns>
        [HttpGet]
        public ActionResult ShowBookFollowCategory(int page = 0, int idCategory = 0, int sortID = 0)
        {
            // hiện tại dữ liệu còn ít, nên test 1 trang có 3 sản phẩm để làm phân trang
            // tương lai sẽ cho người dùng chọn pageSize
            if (Request.QueryString["idcategory"] != null)
            {
                idCategory = int.Parse(Request.QueryString["idcategory"].ToString());
            }
            if (Request.QueryString["page"] != null)
            {
                page = int.Parse(Request.QueryString["page"].ToString());
            }
            //gọi procedure ShowBookFollowCategory
            var allBooks = db.Database.SqlQuery<Book>("ShowBookFollowCategory @categoryID, @sortID",
                new SqlParameter("@categoryID", idCategory), new SqlParameter("@sortID", sortID)).ToList();
            // số book hiển thị
            var books = allBooks.Skip(page * 3).Take(pageSize).ToList();
            var totalPage = (int)Math.Ceiling((decimal)allBooks.Count / pageSize);
            return PartialView("~/Views/Shared/_ShowBookFollowCategory.cshtml", new PaggingModel
            {
                Books = books,
                TotalPage = totalPage,
                IdCategory = idCategory
            });
        }
        /// <summary>
        /// hiển thị dữ liệu
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sortID"></param>
        /// <param name="keySearch"></param>
        [HttpGet]
        public ActionResult ShowBookFollowKeySearch(int page = 0, string keySearch = "")
        {
            //hiện tại dữ liệu còn ít, nên test 1 trang có 1 sản phẩm để làm phân trang
            // tương lai sẽ cho người dùng chọn pageSize
            keySearch = Request.QueryString["keysearch"].ToString();
            if (Request.QueryString["page"] != null)
            {
                page = int.Parse(Request.QueryString["page"].ToString());
            }
            // remove dấu tiếng việt
            string removeUnicodeKeySearch = removeSign.RemoveSign4VietnameseString(keySearch);

            var allBooks = db.Database.SqlQuery<Book>("ShowBookFollowKeySearch @keySearch",
                new SqlParameter("@keySearch", removeUnicodeKeySearch)).ToList();
            var books = allBooks.Skip(page * 3).Take(pageSize).ToList();
            var totalPage = (int)Math.Ceiling((decimal)allBooks.Count / pageSize);

            return PartialView("~/Views/Shared/_ShowBookFollowKeySearch.cshtml", new PaggingModel
            {
                Books = books,
                TotalPage = totalPage
            });
        }

        [HttpGet]
        public ActionResult ShowBookFollowPrice(int page = 0, int priceFrom = 0, int priceTo = 0, int sortID = 0)
        {
            var allBooks = db.Books.Where(x => (x.Price >= priceFrom && x.Price <= priceTo)).ToList();
            var books = allBooks.Skip(page * 3).Take(pageSize).ToList();
            var totalPage = (int)Math.Ceiling((decimal)allBooks.Count / pageSize);

            return PartialView("~/Views/Shared/_ShowBookFollowPrice.cshtml", new PaggingModel
            {
                Books = books,
                TotalPage = totalPage
            });
        }

        [HttpGet]
        public ActionResult ShowPriceBook()
        {
            var allPrice = db.FilterPrices.ToList();
            var totalPrice = allPrice.Count();
            return PartialView("~/Views/Shared/_ShowPriceBook.cshtml", new PaggingModel
            {
                Price = allPrice,
                TotalPrice = totalPrice
            });
        }

        public int GetCountBookFollowPrice(double PriceFrom, double PriceTo)
        {
            return db.Books.Where(x => (x.Price >= PriceFrom && x.Price <= PriceTo)).Count(); ;
        }

        [HttpPost]
        public ActionResult GetInforFilterPrice(int FilterID)
        {
            FilterPrice Infor = db.FilterPrices.Where(x => x.FilterID == FilterID).FirstOrDefault();
            var rslt = new
            {
                priceFrom = Infor.PriceFrom,
                priceTo = Infor.PriceTo
            };
            return Json(rslt, JsonRequestBehavior.AllowGet);
        }
    }
}