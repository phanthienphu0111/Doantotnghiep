using BS_Project.DAO;
using BS_Project.EF;
using BS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Controllers
{
    public class LoginUserController : Controller
    {
        // GET: LoginUser
        private BSDBContext db = new BSDBContext();
        public ActionResult LoginUsernormal()
        {
            return View();
        }

        public ActionResult LoginUser(User model)
        {
            model.Password = CommonConstant.Encrypt(model.Password);
            bool check = new AccountDAO().checkAccount(model.Username, model.Password);
            if (check)
            {
                var roleID = db.Users.Where(a => a.Username == model.Username).FirstOrDefault().UserRoleID;
                Session["roleID"] = roleID;
                if (check && ((int)Session["roleID"] == 2))
                {
                    Session["UserName"] = model.Username;
                    if (model.ImageURL != null)
                    {
                        Session["imageUser"] = model.ImageURL;
                    }
                    else
                    {
                        Session["imageUser"] = "/Content/images/bot_2.jpg";
                    }
                    var dao = db.Users.Where(x => x.Username == model.Username).Select(x => x.UserID).ToList();
                    var name = db.Users.Where(a => a.Username == model.Username && a.Password == model.Password).FirstOrDefault().Username;
                    var blockID = db.Users.Where(a => a.Username == model.Username).FirstOrDefault().isActivated;
                    Session["UserID"] = dao[0].ToString();
                    Session["userHello"] = name;
                    Session["blockID"] = blockID;
                    return Json(new { status = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false }, JsonRequestBehavior.AllowGet);
        }
    }
}