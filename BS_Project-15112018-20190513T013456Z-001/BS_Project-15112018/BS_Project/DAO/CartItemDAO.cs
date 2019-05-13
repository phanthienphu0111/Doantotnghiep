using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS_Project.DAO
{
    public class CartItemDAO
    {
        private BSDBContext db = new BSDBContext();

        /// <summary>
        /// Đếm số lượng đơn hàng của KH đang được giao
        /// Nếu có quá 3 đơn hàng đang trong quá trình giao KH sẽ không được đặt mua tiếp
        /// <param name="userID"></param>
        /// <returns></returns>
        public int CountBookBorrowed(int userID)
        {
            //Tìm toàn bộ những đơn hàng của KH userID
            var order = db.OrdersBooks.Where(x => x.UserID == userID).Select(x => x.Status == 0).Count();

            return order;
        }

        
    }
}