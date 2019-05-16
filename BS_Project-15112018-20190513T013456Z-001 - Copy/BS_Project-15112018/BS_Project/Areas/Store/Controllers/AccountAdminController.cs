using BS_Project.DAO;
using BS_Project.EF;
using BS_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Areas.Store.Controllers
{
    public class AccountAdminController : Controller
    {
        // GET: Store/AccountAdmin
        private BSDBContext db = new BSDBContext();

        // GET: Store/AccountAdmin
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public JsonResult ExecuteLogin(User model)
        {
            model.Password = CommonConstant.HashPassword(model.Password);
            bool check = new AccountDAO().checkAccount(model.Username, model.Password);
            if (check)
            {
                var roleID = db.Users.Where(a => a.Username == model.Username).FirstOrDefault().UserRoleID;
                Session["roleID"] = roleID;
                if (check && ((int)Session["roleID"] == 1))
                {
                    Session["UserName"] = model.Username;
                    return Json(new { status = true });
                }
                else
                {
                    return Json(new { status = false });
                }
            }
            return Json(new { status = false });
        }

        public ActionResult LogOff()
        {
            Session["UserName"] = null;
            Session["roleID"] = null;
            return RedirectToAction("Login", "AccountAdmin");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                using (BSDBContext db = new BSDBContext())
                {
                    var user = db.Users.FirstOrDefault(p => p.Username == model.Username);
                    if (user == null)
                    {
                        model.UserRoleID = 1;
                        model.isActivated = true;
                        model.Password = CommonConstant.HashPassword(model.Password);
                        db.Users.Add(model);
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Message = "Tài khoản " + model.Username + " đã tồn tại";
                        return View();
                    }
                }
                ModelState.Clear();
                ViewBag.Message = "Đăng ký thành công cho tài khoản " + model.Username;
            }
            return RedirectToAction("Login");
        }

        public JsonResult CheckUsernameExist(string username)
        {
            var result = new AccountDAO().CheckAccountExist(username);
            return Json(new
            {
                status = result
            });
        }

        public ActionResult Profile(string searchString, int page = 1, int pageSize = 5)
        {
            if(Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new UserDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        public ActionResult UnActived(int userID)
        {
            var dao = db.Users.Find(userID);
            dao.isActivated = false;
            int rslt = 1;
            try
            {
                db.SaveChanges();
            }catch(Exception ex)
            {
                rslt = -1;
            }
            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Actived(int userID)
        {
            var dao = db.Users.Find(userID);
            dao.isActivated = true;
            int rslt = 1;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                rslt = -1;
            }
            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        ///<summary>
        ///Hiển thị thông tin của 1 Tài khoản người dùng
        ///</summary>
        public async Task<ActionResult> Edit(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if(user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        ///<summary>
        ///Sửa 1 tài khoản người dùng & lưu nó lại
        ///</summary>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(User user, FormCollection formCollection)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            string imageURL = null;
            try
            {
                imageURL = formCollection["txtImageURL"].ToString();
            }
            catch
            {
                imageURL = "/Content/images/Image.jpg";
            }
            try
            {
                if (ModelState.IsValid || user != null)
                {
                    var dao = new AccountDAO();
                    var result = dao.Update(user, imageURL);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Profile");
                }
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            return View(user);
        }
    }
}