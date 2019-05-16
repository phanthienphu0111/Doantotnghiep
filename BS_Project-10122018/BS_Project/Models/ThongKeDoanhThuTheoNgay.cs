using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BS_Project.Models
{
    public class ThongKeDoanhThuTheoNgay
    {
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? Ngay
        {
            get;
            set;
        }
        [DisplayFormat(DataFormatString = "{0:#,##0} VND")]
        public double? DoanhThu
        {
            get;set;
        }
         
        public List<OrdersBook> DanhSachDonHang { get; set; }
    }
}