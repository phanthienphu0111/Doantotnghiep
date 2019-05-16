using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace BS_Project.DAO
{
    public class AccountDAO
    {
        private BSDBContext db = new BSDBContext();

        public bool Insert(User ac)
        {
            try
            {
                db.Users.Add(ac);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool checkAccount(string Username, string Password)
        {
            //var User = db.Users.Where(x => x.Username.CompareTo(Username) == 0);
            //var Pass = db.Users.Where(x => x.Password.CompareTo(Password) == 0);
            //if (User.Count() > 0 && Pass.Count() > 0)
            //{
            //    return true;
            //}
            //else return false;

            var result = db.Users.Where(p => p.Username == Username && p.Password == Password).Count();
            if (result != 0)
            {
                return true;
            }
            else return false;
        }

        public string InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.Username == entity.Username);
            if(user == null)
            {
                try
                {
                    db.Users.Add(entity);
                    db.SaveChanges();
                    return entity.Username;
                }
                catch (DbEntityValidationException e) 
                {
                    throw e;
                }
            }
            else
            {
                return user.Username;
            }
        }

        public bool CheckAccountExist(string username)
        {
            var User = db.Users.Where(x => x.Username.CompareTo(username) == 0);
            if (User.Count() > 0)
            {
                //Tồn tại Username
                return false;
            }
            //Username chưa tồn tại
            else return true;
        }

        ///<summary>
        ///Update thông tin của 1 tài khoản
        ///</summary>
        public bool Update(User user, string imageURL)
        {
            try
            {
                var userOld = db.Users.Find(user.UserID);
                userOld.ImageURL = imageURL;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}