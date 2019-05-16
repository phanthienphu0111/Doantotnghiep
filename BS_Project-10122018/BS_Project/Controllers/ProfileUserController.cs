using BS_Project.DAO;
using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Controllers
{
    public class ProfileUserController : Controller
    {
        private BSDBContext _db = new BSDBContext();
        // GET: ProfileUser
        public ActionResult EditProfile(string userName)
        {
            ViewBag.User = new ProfileUserDAO().viewProfileUser(userName);
            return View(ViewBag.User);
        }
        /// <summary>
        /// method update thong tin cua user gom ngay sinh cua user va file hinh anh cua user dua vao database
        /// </summary>
        /// <param name="userName">tra ve username cua user dang nhap vao he thong</param>
        /// <param name="birthDay">ngay sinh cua username duoc he thong tra ve</param>
        /// <param name="fileName">duong dan ma user update len .tu local </param>
        /// <returns>tra ve thong tin view update xuong database</returns>
        [HttpPost]
        public ActionResult EditProfile(string userName, string Phone, string Address, string returnUrl, DateTime birthDay, HttpPostedFileBase fileName)
        {

            User u = new User();
            u.Username = userName;
            u.Birthday = birthDay.Date;
            u.Phone = Phone;
            u.Address = Address;
            if (fileName == null)
            {
                var dao = new ProfileUserDAO();
                string fileNameUser = Request.Form["fileName"];
                var result = dao.Update(u.Username, u.Birthday, fileNameUser, Phone, Address);
                if (result)
                {
                    if (returnUrl != null)
                        return Redirect(returnUrl);

                    return RedirectToAction("EditProfile", "ProfileUser", new { @userName = userName });
                }
                else
                {
                    return RedirectToAction("ProfileUser", "ProfileUser");
                }
            }
            var allowedExtensions = new[] {
            ".Jpg", ".png", ".jpg", "jpeg"
            };
            u.ImageURL = fileName.FileName;

            var fileNameImage = Path.GetFileNameWithoutExtension(fileName.FileName);
            var ext = Path.GetExtension(fileName.FileName);
            if (ModelState.IsValid)
            {
                if (allowedExtensions.Contains(ext))
                {
                    string name = Path.GetFileNameWithoutExtension(fileNameImage);
                    string userFile = name + "_" + u.Username + ext;

                    var path = Path.Combine(Server.MapPath("~/Content/images/image profile/"), userFile);
                    u.ImageURL = path;

                    var dao = new ProfileUserDAO();
                    var result = dao.Update(u.Username, u.Birthday.Date, userFile, Phone, Address);
                    if (result)
                    {
                        fileName.SaveAs(u.ImageURL);
                        return RedirectToAction("EditProfile", "ProfileUser", new { @userName = userName });
                    }
                    else
                    {
                        return RedirectToAction("ProfileUser", "ProfileUser");
                    }
                }
            }
            return View("EditProfile");
        }

        public PartialViewResult _Sidebar(string userName)
        {
            ViewBag.User = new ProfileUserDAO().viewProfileUser(userName);
            return PartialView();
        }

        public ActionResult OrderHistory(string userName)
        {
            List<OrdersBook> ordersBooks = _db.OrdersBooks.Where(o => o.User != null && o.User.Username == userName).OrderByDescending(m => m.OrderID).ToList();
            return View(ordersBooks);
        }

        [HttpPost]
        public ActionResult CancelOrder(int Id, string returnUrl)
        {
            OrdersBook ordersBook = _db.OrdersBooks.Find(Id);
            ordersBook.Canceled = true;
            ordersBook.Status = 0;
            _db.Entry(ordersBook).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return Redirect(returnUrl);
        }

    }
}