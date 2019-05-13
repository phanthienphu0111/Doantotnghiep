using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BS_Project.Models
{
    public class CommonConstant
    {
        public const string VN_CULTURE = "vi-VN";
        //Session của giỏ hàng
        public static string cartSession = "CartSession";
        //Session của Login
        public static string userHello = "userHello";
        public static string userName = "userName";
        public static string UserID = "UserID";
        public static string blockID = "blockID";
        public static string roleID = "roleID";
        public static string AccessToken = "AccessToken";
        //userRole của khách hàng
        public static int userRoleNormal = 2;
        //userRole của Admin
        public static int userRoleAdmin = 1;
        //userRole của Nhân viên giao hàng
        public static int userRoleShipper = 3;
        //biến trạng thái của account
        public static bool isActived = true;

        /// <summary>
        /// định dạng tiền việt nam
        /// </summary>
        /// <param name="money">số tiền truyền vào</param>
        /// <param name="strFormart">dấu muốn định nghĩa</param>
        /// <returns>VD: 1.000.000 VNĐ</returns>
        public string formatMoney(string money, string strFormart)
        {

            StringBuilder strB = new StringBuilder(money);
            int j = 0;
            string str = "";
            for (int i = strB.Length - 1; i > 0; i--)
            {
                j++;
                if (j % 3 == 0)
                {
                    strB.Insert(i, strFormart);
                    j = 0;
                }
            }

            str = strB.ToString();
            return str;
        }

        /// <summary>
        /// Hàm lấy Thứ của một ngày bất kỳ.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="strFormat"></param>
        /// <returns></returns>
        public static string GetWeekOfDate(DateTime? dateTime, string strFormat)
        {
            string outHtml = string.Empty;
            if (dateTime != null)
            {
                DateTime date = dateTime.Value;
                string weekOfDate = date.GetJstDateTime("dddddd");
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    outHtml = "<span class='weekday-red'>";
                    outHtml += weekOfDate;
                    outHtml += "</span>";
                }
                else if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    outHtml = "<span class='weekday-blue'>";
                    outHtml += weekOfDate;
                    outHtml += "</span>";
                }
                else
                {
                    outHtml = weekOfDate;
                }
                return string.Format("{0} ({1}) ", date.ToString(strFormat), outHtml);
            }
            return outHtml;
        }

        public static string HashPassword(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}