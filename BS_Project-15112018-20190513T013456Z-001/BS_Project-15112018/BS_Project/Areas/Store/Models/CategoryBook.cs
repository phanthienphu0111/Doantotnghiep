using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS_Project.Areas.Store.Models
{
    public class CategoryBook
    {
        public int CategoryID { get; set; }

        public string Name { get; set; }

        public bool? isDeleted { get; set; }
    
        public virtual ICollection<Book> Books { get; set; }
    }
}