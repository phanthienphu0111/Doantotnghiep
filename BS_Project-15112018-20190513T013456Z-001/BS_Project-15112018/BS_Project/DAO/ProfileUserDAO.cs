using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS_Project.DAO
{
    public class ProfileUserDAO
    {
        BSDBContext _db = new BSDBContext();

        /// <summary>
        /// tra ve tat ca cac thong tin cua user qua thong qua account
        /// </summary>
        /// <param name="username">kiem tra user da dang nhap vao he thong qua account</param>
        /// <returns>list cac thong tin cua user</returns>
        public User viewProfileUser(string userName)
        {
            //return db.Users.Find(username);
            var queryProfile = from userProfle in _db.Users
                               where userProfle.Username == userName
                               select userProfle;

            //return _db.Users.Where(x => x.Username.Contains(userName));
            return queryProfile.FirstOrDefault();
        }

        /// <summary>
        /// update thong tin profile vao database
        /// </summary>
        /// <param name="userName">kiem tra user co trong database de update</param>
        /// <param name="birthDay">user chi duoc update birth vao database</param>
        /// <returns>update thanh cong thi tra ve tru con nguoc lai la false</returns>
        public bool Update(string userName, DateTime birthDay, string image, string Phone, string Address)
        {
            try
            {
                var queryProfile = from userProfle in _db.Users
                                   where userProfle.Username == userName
                                   select userProfle;
                if (birthDay == null && image == null)
                {
                    return false;
                }
                else
                {
                    queryProfile.FirstOrDefault().Birthday = birthDay.Date;
                    queryProfile.FirstOrDefault().ImageURL = image;
                    queryProfile.FirstOrDefault().Phone = Phone;
                    queryProfile.FirstOrDefault().Address = Address;
                    _db.SaveChanges();
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
    }
}