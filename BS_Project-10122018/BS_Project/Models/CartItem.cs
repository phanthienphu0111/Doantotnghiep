using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS_Project.Models
{
    [Serializable] /*Tuần tự hóa*/
    public class CartItem
    {
        public Book Books { get; set; }
        public int Quantity { get; set; }
    }
}