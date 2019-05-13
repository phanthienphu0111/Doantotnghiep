using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using BS_Project.EF;
using BS_Project.Models;
using PagedList;
using PagedList.Mvc;
namespace BS_Project.Areas.Store.Controllers
{
    public class StatisticalController : Controller
    {
        BSDBContext db = new BSDBContext();
        // GET: Admin/Statistical
        public ActionResult Index(DateTime? from, DateTime? to)
        {
            List<ThongKeDoanhThuTheoThang> lst = new List<ThongKeDoanhThuTheoThang>();
            for (short i = 1; i <= 12; i++)
            {
                lst.Add(new ThongKeDoanhThuTheoThang() { Thang = i, DoanhThu = (int)db.OrdersBooks.Where(m => m.FoundedDate.HasValue && m.FoundedDate.Value.Year == DateTime.Today.Year && m.FoundedDate.Value.Month == i).Sum(m=>m.OrdersDetails.Sum(tt=>tt.Quantity * (tt.Book.Price ?? 0))).GetValueOrDefault(0)  });
            }
            if(from == null && to == null) {
                ViewBag.lst2 = db.OrdersDetails.Where(o=>o.OrdersBook.Paid == true).GroupBy(m => m.CreatedDate).Select(o => new ThongKeDoanhThuTheoNgay() { Ngay = o.Key, DanhSachDonHang = o.Select(m=>m.OrdersBook).ToList().Distinct().ToList(), DoanhThu = o.Sum(tt => tt.Quantity * tt.Book.Price) }).ToList();
            }
            else {
                ViewBag.lst2 = db.OrdersDetails.Where(o => o.OrdersBook.Paid == true && o.CreatedDate >= from && o.CreatedDate <= to ).GroupBy(m => m.CreatedDate).Select(o => new ThongKeDoanhThuTheoNgay() { Ngay = o.Key, DanhSachDonHang = o.Select(m => m.OrdersBook).ToList().Distinct().ToList(), DoanhThu = o.Sum(tt => tt.Quantity * tt.Book.Price) }).ToList();

            }
            return View(lst);
        }

        public ActionResult QuantityBook(int? page)
        {
            List<QuantityBookViewModel> model = db.Books.Select(b => new QuantityBookViewModel() { TenSach = b.Name, SoLuongNhap = b.TotalQuantity, SoLuongBan = b.OrdersDetails.Where(o=>o.OrdersBook.Paid == true).Sum(o => o.Quantity),  DonGia = b.Price }).ToList();
            return View(model.ToPagedList(page ?? 1, 10));
        }

        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            List<QuantityBookViewModel> model = db.Books.Select(b => new QuantityBookViewModel() { TenSach = b.Name, SoLuongNhap = b.TotalQuantity, SoLuongBan = b.OrdersDetails.Where(o => o.OrdersBook.Paid == true).Sum(o => o.Quantity), DonGia = b.Price }).ToList();
            gv.DataSource = model;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=QuantityBook.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            return View("QuantityBook");
        }
    }
}