using BS_Project.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS_Project.DAO
{
    public class CommentsDAO
    {
        public static BSDBContext db = new BSDBContext();
        public static List<Comment> GetComments(int bookID)
        {
            List<Comment> commentList = null;
            try
            {
                commentList = db.Comments.Where(a => a.BookID.CompareTo(bookID) == 0).OrderByDescending(b => b.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            return commentList;
        }
        public static string getUsername(int user_id)
        {
            return db.Users.Find(user_id).Username;
        }

        public static string getPicture(int user_id)
        {
            return db.Users.Find(user_id).ImageURL;
        }

        public bool AddCmt(Comment cmt)
        {
            try
            {
                string qrInsert = "Insert into Comments (UserID, BookID, Content, CreatedDate, isDeleted) values (" + cmt.UserID + ", " + cmt.BookID + ", '" + cmt.Content + "', getdate(), " + "0)";
                db.Database.ExecuteSqlCommand(qrInsert);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<Comment> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Comment> model = db.Comments.OrderBy(x => x.CreatedDate);
            if(!string.IsNullOrEmpty(searchString))
            {
                model = db.Comments.Where(x => x.Book.Name.Contains(searchString));
            }
            return model.OrderBy(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}