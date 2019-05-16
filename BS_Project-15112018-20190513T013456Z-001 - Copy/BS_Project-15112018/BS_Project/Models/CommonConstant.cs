using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BS_Project.Models
{
    public class CommonConstant
    {
        public const string VN_CULTURE = "vi-VN";
        /// <summary>
        /// Session của Giỏ hàng
        /// </summary>
        public static string cartSession = "CartSession";
        /// <summary>
        /// Tên của người dùng
        /// </summary>
        public static string userHello = "userHello";
        /// <summary>
        /// Username của account
        /// </summary>
        public static string userName = "userName";
        /// <summary>
        /// UserID của account
        /// </summary>
        public static string UserID = "UserID";
        /// <summary>
        /// Lưu biến check Vô hiệu hóa tài khoản
        /// </summary>
        public static string blockID = "blockID";
        /// <summary>
        /// biến check role của account
        /// </summary>
        public static string roleID = "roleID";
        public static string AccessToken = "AccessToken";
        /// <summary>
        /// UserRole của người dùng
        /// </summary>
        public static int userRoleNormal = 2;
        /// <summary>
        /// UserRole của Admin
        /// </summary>
        public static int userRoleAdmin = 1;
        /// <summary>
        /// UserRole của người giao hàng
        /// </summary>
        public static int userRoleShipper = 3;
        /// <summary>
        /// biến Check  Vô hiệu hóa tài khoản
        /// </summary>
        public static bool isActived = true;
        //Encrypt and decrypt
        public string plaintext = "123456";

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

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        ///<summary>
        ///Class lưu lại thông tin đăng nhập
        ///</summary>
        public class LoginClass
        {
            /// <summary>
            /// Mã Hóa đơn
            /// </summary>
            public string SessionRole { get; set; }

            /// <summary>
            /// Session lưu blockID
            /// </summary>
            public bool SessionBlockID { get; set; }

            /// <summary>
            /// Session lưu UserName
            /// </summary>
            public string SessionUsername { get; set; }
        }

        /// <summary>
        /// Class lưu thông tin để gửi sang Action Paypal
        /// </summary>
        public class InforPaypal
        {
            /// <summary>
            /// Mã Hóa đơn
            /// </summary>
            public int OrderId { get; set; }

            /// <summary>
            /// Tổng giá trị đơn hàng
            /// </summary>
            public double Total { get; set; }

        }
    }
}