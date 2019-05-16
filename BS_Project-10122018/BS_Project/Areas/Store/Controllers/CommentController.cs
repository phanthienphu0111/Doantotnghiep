using BS_Project.DAO;
using BS_Project.EF;
using BS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;

namespace BS_Project.Areas.Store.Controllers
{
    public class CommentController : Controller
    {
        private static BSDBContext db = new BSDBContext();
        // GET: Store/Comment
        public ActionResult Index(string searchString, int page = 1, int pageSize = 4)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            var dao = new CommentsDAO();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View("~/Areas/Store/Views/Comment/Index.cshtml", model);
        }

        ///<summary>
        ///Hiển thị trang gửi phản hồi bình luận cho Khách hàng
        ///</summary>
        public async Task<ActionResult> Reply(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment model = await db.Comments.FindAsync(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult SendEmailFeedBack(string bookname, string fullname, string replycontent, string email)
        {
            int result = 1;
            //Gửi mail thông báo đơn hàng cho Khách hàng
            try
            {
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Store/Views/template/newReply.cshtml"));
                content = content.Replace("{{CustomerName}}", fullname);
                content = content.Replace("{{BookName}}", bookname);
                content = content.Replace("{{content}}", replycontent);
                new MailHelper().SendMail(email, "Đơn hàng mới từ QT BookStore", content, "Thông báo Đơn Đặt hàng từ QT BookStore");
            }
            catch(Exception ex)
            {
                result = -1;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}