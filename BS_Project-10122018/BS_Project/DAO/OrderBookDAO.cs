using BS_Project.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Project.DAO
{
    public class OrderBookDAO
    {
        public BSDBContext db = new BSDBContext();

        public static BSDBContext data = new BSDBContext();
        public OrdersBook Get(int id)
        {
            return db.OrdersBooks.Find(id);
        }

        public List<int> GetOrderID(int userID)
        {
            return db.OrdersBooks.Where(x => x.UserID == userID).Select(x => x.OrderID).ToList();
        }

        public List<int> GetBookID(int orderID)
        {
            return db.OrdersDetails.Where(x => x.OrderID == orderID).Select(x => x.BookID).ToList();
        }

        public int GetID()
        {
            return db.OrdersBooks.Max(x => x.OrderID);
        }

        /// <summary>
        /// Đếm số lượng sách mà user đã mua
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int CountBookBuyed(int userID)
        {
            var bookBuyed = 0;
            //Lấy danh sách Order mà User đã mượn
            var orderID = GetOrderID(userID);
            var listBookID = GetBookID(Convert.ToInt32(orderID));
            foreach (var item in listBookID)
            {
                //Lấy danh sách chi tiết các sách mà User đã mua trong 1 Order
                bookBuyed++;
            }
            return bookBuyed;
        }

        public bool Insert(OrdersBook order)
        {
            try
            {
                db.OrdersBooks.Add(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<OrdersBook> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<OrdersBook> model = db.OrdersBooks;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = db.OrdersBooks.Where(x => x.User.Username.Contains(searchString) || (SqlFunctions.StringConvert((double)x.OrderID).Contains(searchString)));
            }
            return model.OrderByDescending(x => x.OrderID).ToPagedList(page, pageSize);
        }

        public static int CanceledOrder(int userID)
        {
            return data.OrdersBooks.Where(x => x.UserID == userID && x.Canceled == true).Count();
        }

        public IPagedList<OrdersBook> ListAllPagingFilter(int sortID, string searchString, int page, int pageSize)
        {
            List<OrdersBook> model = db.OrdersBooks.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                model = db.OrdersBooks.Where(x => x.User.Username.Contains(searchString) || (SqlFunctions.StringConvert((double)x.OrderID).Contains(searchString))).ToList();
            }
            return model.OrderBy(x => x.OrderID).Where(x => x.Status == sortID).ToPagedList(page, pageSize);
        }

        public double chuyenVNDtoUSD(double total)
        {
            return (total*1.0 / 23000);
        }
    }
}