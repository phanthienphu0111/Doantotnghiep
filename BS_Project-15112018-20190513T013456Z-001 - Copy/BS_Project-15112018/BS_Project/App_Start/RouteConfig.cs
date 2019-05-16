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

            routes.MapRoute(
                name: "Cart",
                url: "gio-hang",
                defaults: new { controller = "CartItem", action = "Index", id = UrlParameter.Optional },
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
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "BS_Project.Controllers" }
            );
        }
    }
}
