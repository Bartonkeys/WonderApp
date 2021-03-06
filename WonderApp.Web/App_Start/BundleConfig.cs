﻿using System.Web;
using System.Web.Optimization;

namespace WonderApp.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.3.js",
                        "~/Scripts/select2.js",
                        "~/Scripts/jquery.are-you-sure.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
                        //"~/Scripts/MvcFoolproofJQueryValidation.min.js",
                        //"~/Scripts/MvcFoolproofValidation.min.js",
                        //"~/Scripts/mvcfoolproof.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tagSearchJs").Include(
                       "~/Scripts/select2.js",
                       "~/Scripts/tagSearch.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                     "~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/location").Include(
                      "~/App/Location/location.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                "~/Plugins/DataTables-1.10.5/media/js/jquery.dataTables.js",
                "~/Plugins/DataTables-1.10.5/extensions/ColumnFilter/yadcf-0.8.6/jquery.dataTables.yadcf.js"));

            bundles.Add(new ScriptBundle("~/bundles/isloading").Include(
                      "~/Scripts/jquery.isloading.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                      "~/Scripts/toastr.min.js"));

            bundles.Add(new StyleBundle("~/Content/css_bundle").Include(
                  "~/Content/css/bootstrap.css",
                   "~/Content/css/bootstrap-datepicker.css",
                  "~/Content/css/site.css",
                  "~/Content/css/select2.css",
                  "~/Content/css/font-awesome/css/font-awesome.min.css",
                  "~/Content/css/isloading.css",
                  "~/Content/css/toastr.min.css"));

            bundles.Add(new StyleBundle("~/Content/css_bundle_datatables").Include(
                "~/Plugins/DataTables-1.10.2/media/css/jquery.dataTables.css",
               "~/Plugins/DataTables-1.10.2/extensions/ColumnFilter/yadcf-0.8.6/jquery.dataTables.yadcf.css"));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
