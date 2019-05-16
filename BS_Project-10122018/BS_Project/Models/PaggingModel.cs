using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BS_Project.EF;
using PagedList;

namespace BS_Project.Models
{
    public class PaggingModel
    {
        public List<Book> Books { get; set; }
        public List<Category> Categories { get; set; }
        public int TotalPage { get; set; }
        public int ToTalCategories { get; set; }
        public int IdCategory { get; set; }
        public int idPublisher { get; set; }
        public int TotalPrice { get; set; }
    }

}