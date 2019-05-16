using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Project.DAO
{
    public class OrdersDetailsDAO
    {
        private BSDBContext db = new BSDBContext();

        public bool Insert(OrdersDetail temp)
        {
            try
            {
                db.OrdersDetails.Add(temp);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public object GetOrder(int userID)
        {
            object orderLst = null;
            try
            {
                orderLst = (from book in db.OrdersBooks
                            join detail in db.OrdersDetails on book.OrderID equals detail.OrderID
                            where book.UserID == userID
                            select new
                            {
                                bookID = detail.BookID,
                                orderID = book.OrderID,
                                userID = book.UserID,
                                foundDate = detail.CreatedDate,
                                quantity = detail.Quantity
                            }).ToList();
                return orderLst;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
    }
}