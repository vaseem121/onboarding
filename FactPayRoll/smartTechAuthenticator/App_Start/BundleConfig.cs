using System.Web;
using System.Web.Optimization;

namespace smartTechAuthenticator
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.4.1.js"));

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

            #region Serve static library

            bundles.Add(new ScriptBundle("~/validate/js").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.js"));
            bundles.Add(new ScriptBundle("~/toaster/js").Include(
                "~/Scripts/toastr.js",
                "~/Scripts/toastr.min.js"));

            //Js library files
            bundles.Add(new ScriptBundle("~/bundle/js").Include(
                "~/Content/assets/vendor/php-email-form/validate.js",
                "~/Content/assets/vendor/swiper/swiper-bundle.min.js",
                "~/Content/assets/vendor/isotope-layout/isotope.pkgd.min.js",
                "~/Content/assets/vendor/glightbox/js/glightbox.min.js",
                "~/Content/assets/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Content/assets/vendor/aos/aos.js",
                "~/Content/assets/vendor/purecounter/purecounter.js"
                ));

            #endregion  Serve static library

            bundles.Add(new ScriptBundle("~/bundle/Account").Include("~/Scripts/Account/Account.js"));
            bundles.Add(new ScriptBundle("~/bundle/pdf-generator").Include("~/Scripts/pdf-generator/jspdf.min.js"));
        }
    }
}
