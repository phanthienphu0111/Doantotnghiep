using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BS_Project
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Routes User
            routes.MapRoute(
                name: "Cart",
                url: "gio-hang",
                defaults: new { controller = "CartItem", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Controllers" }
            );
            routes.MapRoute(
                name: "Index",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Controllers" }
            );
            routes.MapRoute(
                name: "Add Cart",
                url: "them-gio-hang",
                defaults: new { controller = "CartItem", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Controllers" }
            );
            routes.MapRoute(
                name: "Payment",
                url: "thanh-toan",
                defaults: new { controller = "OrderBook", action = "Payment", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Controllers" }
            );
            routes.MapRoute(
                name: "Payment Success",
                url: "hoan-thanh",
                defaults: new { controller = "OrderBook", action = "Succeed", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Controllers" }
            );
            routes.MapRoute(
                name: "BookDetail",
                url: "sach-chi-tiet/{id}",
                defaults: new { controller = "Home", action = "BookDetail", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Controllers" }
            );
            #endregion

            #region Routes Admin Page
            ///<summary>
            ///Routes Admin Page
            ///</summary>
            routes.MapRoute(
                name: "Admin",
                url: "trang-quan-tri",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            routes.MapRoute(
                name: "AdminLogin",
                url: "dang-nhap-quan-tri",
                defaults: new { controller = "AccountAdmin", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            routes.MapRoute(
                name: "Statistical_Total",
                url: "thong-ke-doanh-thu",
                defaults: new { controller = "Statistical", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            routes.MapRoute(
                name: "Statistical_Quantity",
                url: "thong-ke-so-luong-sach",
                defaults: new { controller = "Statistical", action = "QuantityBook", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            //routes.MapRoute(
            //    name: "Account",
            //    url: "tai-khoan",
            //    defaults: new { controller = "AccountAdmin", action = "Profile", id = UrlParameter.Optional },
            //    namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            //);
            routes.MapRoute(
                name: "Book",
                url: "sach",
                defaults: new { controller = "Book", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            routes.MapRoute(
                name: "Category",
                url: "the-loai",
                defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            routes.MapRoute(
                name: "Publisher",
                url: "nha-xuat-ban",
                defaults: new { controller = "Publisher", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            routes.MapRoute(
                name: "Order",
                url: "don-hang",
                defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            routes.MapRoute(
                name: "Comment",
                url: "nhan-xet",
                defaults: new { controller = "Comment", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            routes.MapRoute(
                name: "EditAccount",
                url: "sua-tai-khoan/{id}",
                defaults: new { controller = "AccountAdmin", action = "Edit", id = UrlParameter.Optional },
                namespaces: new[] { "BS_Project.Areas.Store.Controllers" }
            );
            #endregion

            ///<summary>
            ///Routes Defaults
            ///</summary>
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "BS_Project.Controllers" }
            );
        }
    }
}
