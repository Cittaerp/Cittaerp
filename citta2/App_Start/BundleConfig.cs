using System.Web;
using System.Web.Optimization;

namespace CittaErp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquerybundle").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-session-timeout.min.js"
                        ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jsuserbundle").Include(
                      "~/Scripts/bootbox.js",
                      "~/Scripts/CittaScripts/hide.js",
                        "~/Scripts/CittaScripts/menu.js",
                        "~/Scripts/CittaScripts/spin.js",
                       "~/Scripts/autoNumeric-min.js",
                       //"~/Scripts/CittaScripts/cescript1.js",
                       // "~/Scripts/CittaScripts/cescript3.js",
                         "~/Scripts/Anchor1/pescript4.js",
                       //   "~/Scripts/CittaScripts/ceuscript.js",
                        "~/Scripts/CittaScripts/parascript.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap*",
                      "~/Content/site.css",
                      "~/Content/themes/base/*.css",
                      "~/Content/fa_awesome/*.css",
                      "~/Content/user1.css",
                      "~/Content/menu.css"
                      //"~/Content/alert.css"
                      ));

            bundles.Add(new StyleBundle("~/dataTables/bundles/cssbundle").Include(
                            "~/Datatables/dataTables.min.css"
                            //"~/DataTables/dataTables.bootstrap.min.css"
                            ));

            bundles.Add(new ScriptBundle("~/dataTables/bundles/js1bundle").Include(
                        "~/Datatables/datatables.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/js2bundle").Include(
                "~/Scripts/Anchor1/get5.min.js",
                "~/Scripts/Anchor1/commonf.min.js",
                "~/Scripts/Anchor1/spin.min.js"
                ));

        }
    }
}
