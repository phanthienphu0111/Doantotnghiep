using BS_Project.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace BS_Project.DAO
{
    public class UserDAO
    {
        private BSDBContext db = new BSDBContext();

        public User GetUserById(int userId)
        {
            return db.Users.Find(userId);
        }
        public IQueryable<string> getUserID(int userID)
        {
            return db.Users.Where(x => x.UserID == userID).Select(x => x.Username).Distinct();
        }

        public string getName(int userID)
        {
            var a = db.Users.Where(x => x.UserID == userID).FirstOrDefault();
            return a.FullName;
        }

        public string getPhone(int userID)
        {
            var a = db.Users.Where(x => x.UserID == userID).FirstOrDefault();
            if (a != null)
            {
                return a.Phone;
            }
            else return "";
        }

        public string getAddress(int userID)
        {
            var a = db.Users.Where(x => x.UserID == userID).FirstOrDefault();
            if(a != null)
            {
                return a.Address;
            }
            else return "";
        }

        public string getEmail(int userID)
        {
            var a = db.Users.Where(x => x.UserID == userID).FirstOrDefault();
            if(a != null)
            {
                return a.Email;
            }
            else return "";
        }

        ///<summary>
        ///Lấy ra danh sách Tài khoản của User
        ///</summary>
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<User> model = db.Users.Where(x => x.UserRoleID == 2);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = db.Users.Where(x => (SqlFunctions.StringConvert((double)x.UserID).Contains(searchString)) || x.Username.Contains(searchString) || x.FullName.Contains(searchString) || x.Phone.Contains(searchString) || x.Email.Contains(searchString));
            }
            return model.OrderBy(x => x.UserID).ToPagedList(page, pageSize);
        }
    }
}