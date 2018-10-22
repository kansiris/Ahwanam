using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace MaaAahwanam.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //vmp masterpage scripts
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "https://code.jquery.com/ui/1.12.1/jquery-ui.js",
                "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/semantic").Include(
                "~/newdesignstyles/Scripts/semantic.js",
                "~/newdesignstyles/Scripts/semantic.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"));

            //vdb scripts
            bundles.Add(new ScriptBundle("~/bundles/vdb").Include(
                "~/Scripts/vdb1.js",
                "https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/CSSWeb/js/jquery-1.9.1.js",
                      "~/CSSWeb/Alert/bootstrap-dialog.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery2").Include(
                      "~/CSSWeb/js/jquery-ui.js",
                      "~/CSSWeb/js/select2.min.js",
                      "~/CSSWeb/js/jquery.easing.min.js",
                      "~/CSSWeb/js/menu.js",
                      "~/CSSWeb/js/stickynavigation.js",
                      "~/CSSWeb/js/wow.min.js",
                      "~/CSSWeb/js/masterslider.min.js",
                      "~/CSSWeb/js/nicescroll.min.js",
                      "~/CSSWeb/js/jquery.magnific-popup.min.js",
                      "~/CSSWeb/js/tabs.js",
                      "~/CSSWeb/js/owl.carousel.min.js",
                      "~/CSSWeb/js/plugins.js",
                      "~/CSSWeb/js/validations.js",
                      "~/CSSWeb/js/jquery.counterup.min.js",
                      "~/CSSWeb/js/feedback.js",
                      "~/CSSWeb/js/rater.js",
                      "~/CSSWeb/js/timeline.js",
                      "~/CSSWeb/js/responsive-tables.js",
                      "~/CSSWeb/js/jquery.shorten.js",
                      "~/CSSWeb/js/smoothproducts.min.js",
                      "~/CSSWeb/js/jquery.datetimepicker.js",
                      "~/CSSWeb/js/waypoints.min.js",
                      "~/CSSWeb/Alert/alert.js",
                      "~/CSSWeb/AutoComplete/AutoComplete.js"));

            bundles.Add(new StyleBundle("~/CSSWeb/css").Include(
                      "~/CSSWeb/css/masterslider.css",
                      "~/CSSWeb/css/ms_style.css",
                      "~/CSSWeb/css/timeline.css",
                      "~/CSSWeb/css/ms-layers-style.css",
                      //"~/CSSWeb/css/owl.carousel.css",
                      //"~/CSSWeb/css/hover.css",
                      //"~/CSSWeb/css/select2.css",
                      //"~/CSSWeb/css/menu.css",
                      "~/CSSWeb/css/tabs.css",
                      //"~/CSSWeb/css/magnific-popup.css",
                      //"~/CSSWeb/css/aileron.css",
                      "~/CSSWeb/css/themify-icons.css",
                      //"~/CSSWeb/css/animate.min.css",
                      "~/CSSWeb/Alert/alert.css",
                      "~/CSSWeb/css/smoothproducts.css",
                      "~/CSSWeb/css/maa-aahwanam.css",
                      "~/CSSWeb/css/responsive.css",
                      "~/CSSWeb/Alert/bootstrap1.css"));

            bundles.Add(new StyleBundle("~/CSSWeb/AutoComplete").Include(
                         "~/CSSWeb/AutoComplete/AutocompleteStyle.css"));

            //Html Tables sorting,pagination
            bundles.Add(new StyleBundle("~/CSSWeb/bootstrap/css").Include(
                      "~/CSSWeb/bootstrap/css/Datatable_css.css"));

            bundles.Add(new ScriptBundle("~/assets/plugins").Include(
                     //"~/assets/plugins/jquery-2.0.3.min.js",
                     //"~/assets/plugins/bootstrap/js/bootstrap.min.js",
                     "~/assets/plugins/dataTables/jquery.dataTables.js",
                     "~/assets/plugins/dataTables/dataTables.bootstrap.js"
                     ));
        }
    }
}