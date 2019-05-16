using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BS_Project.EF;
namespace BS_Project.Models
{
    public class QuantityBookViewModel
    {
        public string TenSach { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0}", NullDisplayText = "0")]
        public double? DonGia { get; set; }

        public int SoLuongTon { get; set; }

        [DisplayFormat(NullDisplayText = "0")]
        public int? SoLuongBan { get; set; }

        public int? SoLuongNhap
        {
            get
            {
                if (this.SoLuongBan == 0 ||this.SoLuongBan == null)
                {
                    return this.SoLuongTon;
                }
                else return this.SoLuongBan + this.SoLuongTon;
            }
        }
    }
}