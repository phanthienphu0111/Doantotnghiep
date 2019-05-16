using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS_Project.Controllers
{
    public class AuthorController : Controller
    {
        private BSDBContext db = null;
        // GET: Author
        /// <summary>
        /// lay thong tin tac gia
        /// </summary>
        /// <param name="bookID"></param>
        /// <returns></returns>
        public PartialViewResult returnAuthor(int bookID)
        {
            db = new BSDBContext();
            var author = db.Books.Find(bookID).Authors.ToList();
            return PartialView(author);
        }
    }
}