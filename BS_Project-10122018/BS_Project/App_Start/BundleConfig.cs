using System.Web;
using System.Web.Optimization;

namespace BS_Project
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/core").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/style.css",
                      "~/Content/font-awesome.css",
                      "~/Content/easy-responsive-tabs.css",
                      "~/Content/LoginPopup/Log.css",
                      "~/Content/datepicker.css",
                      "~/Content/bootstrap-social.css"));

            bundles.Add(new StyleBundle("~/bundles/javascript_layout_before").Include(
                      "~/Scripts/jquery-1.10.2.min.js",
                      "~/Scripts/jquery.alerts.js",
                      "~/Scripts/registerUser.js"
                      ));
            BundleTable.EnableOptimizations = true;
        }
    }
}
