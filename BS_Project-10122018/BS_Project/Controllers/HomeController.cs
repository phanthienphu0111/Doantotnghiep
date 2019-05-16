using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BS_Project.DAO;
using System.Web.Mvc;
using BS_Project.EF;
using BS_Project.Models;
using System.Data.SqlClient;
using Facebook;
using System.Configuration;
using static BS_Project.Models.CommonConstant;

namespace BS_Project.Controllers
{
    public class HomeController : Controller
    {
        private BSDBContext db = new BSDBContext();
        CommonConstant cmCon = new CommonConstant();
        RemoveSignVNModel rmS = new RemoveSignVNModel();
        public ActionResult Index()
        {
            if(Session["UserName"] != null && ((int)Session["roleID"] == 1))
            {
                Session.Abandon();
                Session.RemoveAll();
                return RedirectToAction("LoginUsernormal", "LoginUser");
            }
            List<Book> rs = db.Books.Take(8).OrderByDescending(x => x.ViewCount).ToList();
            ViewData["listHot"] = rs;
            List<Book> xs = db.Books.Take(4).OrderByDescending(x => x.PublicationDate).ToList();
            ViewData["listNew"] = xs;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public PartialViewResult Publisher()
        {
            List<Publisher> list = db.Publishers.ToList<Publisher>();
            return PartialView(list);
        }

        public PartialViewResult Categories()
        {
            List<Category> list = db.Categories.ToList();
            ViewData["List"] = list;
            return PartialView(list);
        }

        [HttpGet] 
        public ActionResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Login(User ac)
        {
            ac.Password = CommonConstant.Encrypt(ac.Password);
            using (BSDBContext db = new BSDBContext())
            {
                var result = db.Users.Where(p => p.Username == ac.Username && p.Password == ac.Password).Count();
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại hoặc Tên đăng nhập, mật khấu bị sai.");
                }
                else
                {
                    var userID = db.Users.Where(a => a.Username == ac.Username).FirstOrDefault().UserID;
                    var name = db.Users.Where(a => a.Username == ac.Username && a.Password == ac.Password).FirstOrDefault().Username;
                    var blockID = db.Users.Where(a => a.Username == ac.Username).FirstOrDefault().isActivated;
                    var roleID = db.Users.Where(a => a.Username == ac.Username).FirstOrDefault().UserRoleID;
                    Session["userHello"] = name;
                    Session["userName"] = ac.Username;
                    Session["UserID"] = userID;
                    Session["blockID"] = blockID;
                    Session["roleID"] = roleID;
                    return Redirect(Request.Url != null ? Request.Url.ToString() : Url.Action("Index"));
                }
            }
            return View();
        }

        [HttpGet]
        public JsonResult LoginAjax(string user_name, string pass)
        {
            pass = CommonConstant.Encrypt(pass);
            using (BSDBContext db = new BSDBContext())
            {
                var result = db.Users.Where(p => p.Username == user_name && p.Password == pass).Count();
                if (result == 0)
                {
                    object jsondata = new LoginClass
                    {
                        SessionRole = null,
                        SessionUsername = null
                    };
                    return Json(jsondata, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var roleID = db.Users.Where(a => a.Username == user_name).FirstOrDefault().UserRoleID;
                    if (roleID == 1)
                    {
                        object jsondata = new LoginClass
                        {
                            SessionRole = null,
                            SessionUsername = null
                        };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var userID = db.Users.Where(a => a.Username == user_name).FirstOrDefault().UserID;
                        var name = db.Users.Where(a => a.Username == user_name && a.Password == pass).FirstOrDefault().Username;
                        var blockID = db.Users.Where(a => a.Username == user_name).FirstOrDefault().isActivated;
                        Session["userHello"] = name;
                        Session["userName"] = user_name;
                        Session["UserID"] = userID;
                        Session["blockID"] = blockID;
                        Session["roleID"] = roleID;

                        object jsondata = new LoginClass
                        {
                            SessionRole = Session["roleID"].ToString(),
                            SessionBlockID = Convert.ToBoolean(Session["blockID"]),
                            SessionUsername = Session["userName"].ToString()
                        };
                        return Json(jsondata, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Register(User account)
        {
            if (account.Username == null || account.Password == null)
            {
                return View();
            }
            if(ModelState.IsValid)
            {
                using (BSDBContext db = new BSDBContext())
                {
                    var user = db.Users.FirstOrDefault(p => p.Username == account.Username);
                    if (user == null)
                    {
                        account.UserRoleID = 2;
                        account.isActivated = true;
                        account.Password = CommonConstant.Encrypt(account.Password);
                        account.Birthday = DateTime.Now;
                        db.Users.Add(account);
                        db.SaveChanges();
                    }
                    else{
                        ViewBag.Message = "UserName already exists" + account.Username;
                        return View();
                    }
                }
                ModelState.Clear();
                ViewBag.Message = "Successfully Registered Mr. " + account.Username;
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public string FormatMoney(string money)
        {
            return cmCon.formatMoney(money, ".");
        }

        /// <summary>
        /// Ham BookDetail co tham so truyen vao la id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult BookDetail(int id)
        {
            var model = db.Books.Find(id);
            // Tăng số lần xem
            model.ViewCount++;
            db.SaveChanges();
            // Lấy cookie cũ tên views
            var views = Request.Cookies["views"];
            // Nếu chưa có cookie cũ -> tạo mới
            if (views == null)
            {
                views = new HttpCookie("views");
            }
            // Bổ sung mặt hàng đã xem vào cookie
            views.Values[id.ToString()] = id.ToString();
            // Đặt thời hạn tồn tại của cookie
            views.Expires = DateTime.Now.AddMonths(1);
            // Gửi cookie về client để lưu lại
            Response.Cookies.Add(views);
            // Lấy List<int> chứa mã hàng đã xem từ cookie
            var keys = views.Values.AllKeys.Select(k => int.Parse(k)).ToList();

            ViewBag.Views = db.Books.Where(p => keys.Contains(p.BookID));
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstant.cartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            ViewBag.Status = Session["userID"];
            ViewBag.BookBorrowed = Session["BookBorrowed"];

            return PartialView(list);
        }
        public ActionResult Logout()
        {
            Session["userName"] = null;
            Session["userID"] = null;
            Session["OrderCart"] = null;
            Session["userHello"] = null;
            Session["blockID"] = null;
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Search(string keysearch = "")
        {
            return RedirectToAction("ShowBook", "Book", new { condition = 4, keysearch = keysearch });
        }

        [HttpPost]
        public JsonResult SearchAutoComplete(string keySearch = "")
        {
            List<string> _json = new List<string>();
            string removeUnicodeKeySearch = rmS.RemoveSign4VietnameseString(keySearch);

            var allBooks = db.Database.SqlQuery<Book>("ShowBookFollowKeySearch @keySearch",
                new SqlParameter("@keySearch", removeUnicodeKeySearch)).ToList();
            // lấy ra 1 list danh sách tên các sách
            foreach (var item in allBooks)
            {
                _json.Add(item.Name);
            }
            return Json(_json);
        }

        [HttpPost]
        public ActionResult GetComments(int bookID)
        {
            object jsonData = GetDataComments(bookID);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private object GetDataComments(int bookID)
        {
            IEnumerable<Comment> commentList = CommentsDAO.GetComments(bookID);
            object jsonData = (from d in commentList
                               select new {
                                   comment_id = d.CommentID,
                                   book_id = d.BookID,
                                   user_id = d.UserID,
                                   username = CommentsDAO.getUsername(d.UserID),
                                   picture = CommentsDAO.getPicture(d.UserID),
                                   folder_id = d.FolderID,
                                   isLike = d.isLike, 
                                   content = d.Content,
                                   date = string.Format("{0} {1}", CommonConstant.GetWeekOfDate(d.CreatedDate, "dd/MM/yyyy"), d.CreatedDate.ToString("HH:mm")),
                               }).OrderBy(x => x.date).ToList();
            return jsonData;
        }

        [HttpPost]
        public ActionResult GetOrderbookByUserID(int userID)
        {
            if(Session["UserID"].ToString() != "")
            {
                object jsonData = GetInforOrder(userID);
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        private object GetInforOrder(int userID)
        {
            var dao = new OrdersDetailsDAO();
            object orderbookLst = dao.GetOrder(userID);
            return orderbookLst;
        }

        [HttpPost]
        public ActionResult RegisterComments(Comment comments, int bookID)
        {
            if (!SetComments(comments))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            if(!AddComments(comments))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            object jsonData = new
            {
                cmtData = GetDataComments(bookID)
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RegisterReplyComments(Comment comments, int bookID)
        {
            if (!SetReplyComments(comments))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            if (!AddReplyComment(comments))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            object jsonData = new
            {
                cmtData = GetDataComments(comments.BookID)
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private bool SetComments(Comment a)
        {
            if(Session["UserID"].ToString() == null)
            {
                return false;
            }
            string b = Session["UserID"].ToString();
            a.UserID = Convert.ToInt32(b);
            a.BookID = a.BookID;
            a.Content = HttpUtility.HtmlDecode(a.Content);
            a.CreatedDate = a.CreatedDate;
            a.isLike = 0;
            a.FolderID = 0;
            return true;
        }

        private bool SetReplyComments(Comment a)
        {
            if (Session["UserID"].ToString() == null)
            {
                return false;
            }
            string b = Session["UserID"].ToString();
            a.UserID = Convert.ToInt32(b);
            a.BookID = a.BookID;
            a.Content = HttpUtility.HtmlDecode(a.Content);
            a.CreatedDate = a.CreatedDate;
            a.isLike = 0;
            a.FolderID = a.FolderID;
            return true;
        }

        public static bool AddComments(Comment cmt)
        {
            CommentsDAO dao = new CommentsDAO();
            bool add = dao.AddCmt(cmt);
            return add;
        }

        public static bool AddReplyComment(Comment cmt)
        {
            CommentsDAO dao = new CommentsDAO();
            bool add = dao.AddReplyCmt(cmt);
            return add;
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            var expires = result.expires;
            Session[CommonConstant.AccessToken] = accessToken;
            
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=id,name,email,first_name,birthday,last_name,gender,address");
                string Avatar = string.Format("https://graph.facebook.com/{0}/picture", me.id);
                string email = me.email;
                string id = me.id;
                string fullName = me.first_name + me.last_name;
                string birth = me.birthday;

                //Thêm tài khoản vào bảng User
                var user = new User();
                user.Username = me.id;
                user.FullName = fullName;
                user.Birthday = DateTime.Now;
                user.Email = email;
                user.ImageURL = Avatar;
                user.UserRoleID = CommonConstant.userRoleNormal;
                user.isActivated = CommonConstant.isActived;
                user.Password = CommonConstant.Encrypt("123456");
                user.Address = me.address;

                var rslt = new AccountDAO().InsertForFacebook(user);
                if(rslt != null)
                {
                    var userID = db.Users.Where(a => a.Username == user.Username).FirstOrDefault().UserID;
                    var name = db.Users.Where(a => a.Username == user.Username && a.Password == user.Password).FirstOrDefault().Username;
                    var blockID = db.Users.Where(a => a.Username == user.Username).FirstOrDefault().isActivated;
                    var roleID = db.Users.Where(a => a.Username == user.Username).FirstOrDefault().UserRoleID;
                    Session[CommonConstant.userHello] = name;
                    Session[CommonConstant.userName] = user.Username;
                    Session[CommonConstant.UserID] = userID;
                    Session[CommonConstant.blockID] = blockID;
                    Session[CommonConstant.roleID] = roleID;
                }
            }
            if (Session[CommonConstant.userHello] != null)
                return RedirectToAction("EditProfile", "ProfileUser", new { userName = Session[CommonConstant.userName] });
            else
                return Redirect("/");
        }
        public ActionResult LoginFaceBook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        /// <summary>
        /// Kiểm tra Tên đăng nhập có tồn tại hay không?
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public JsonResult CheckUsernameExist(string username)
        {
            var result = new AccountDAO().CheckAccountExist(username);
            return Json(new
            {
                status = result
            });
        }

        public ActionResult Redirect()
        {
            return RedirectToAction("Index","Home");
        }
    }
}