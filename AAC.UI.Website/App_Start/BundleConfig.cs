using System.Web;
using System.Web.Optimization;

namespace AAC.UI.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles( BundleCollection bundles )
        {
            bundles.Add( new ScriptBundle( "~/bundles/jquery" ).Include(
                        "~/Scripts/jquery-{version}.js" ) );
            bundles.Add( new ScriptBundle( "~/bundles/jqueryui" ).Include(
                        "~/Scripts/jquery-ui.js" ) );
            bundles.Add( new ScriptBundle( "~/bundles/jdtpicker" ).Include(
                        "~/Scripts/jquery.datetimepicker.full.js" ) );
            bundles.Add( new ScriptBundle( "~/bundles/jquerymigrate" ).Include(
                     "~/Scripts/jquery-migrate-1.2.1.js" ) );

            bundles.Add( new ScriptBundle( "~/bundles/jqueryval" ).Include(
                        "~/Scripts/jquery.validate*" ) );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add( new ScriptBundle( "~/bundles/modernizr" ).Include(
                        "~/Scripts/modernizr-*" ) );

            bundles.Add( new ScriptBundle( "~/bundles/bootstrap" ).Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js" ) );

            //bundles.Add( new StyleBundle( "~/Content/css" ).Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css" ) );

            bundles.Add( new StyleBundle( "~/Content/css" ).Include(
                      "~/Content/bootstrap.css" ) );

            //********
            //*JQGRID*
            //********
            //CSS
            bundles.Add( new StyleBundle( "~/Content/jquery-grid" ).Include(
                "~/Content/jquery-grid/ui.jqgrid.css",
                "~/Content/jquery-ui/redmond/jquery-ui-1.8.14.custom.css" ) );
            //JS
            bundles.Add( new ScriptBundle( "~/bundles/jquery-grid" ).Include(
                "~/Scripts/jquery-grid/grid.locale-en.js",
                "~/Scripts/jquery-grid/jquery.jqGrid.min.js" ) );
            //********

            //***********
            //Design CSS*
            //***********
            bundles.Add( new StyleBundle( "~/Content/sitecss" ).Include(
                "~/css/MENUstyle.css",
                "~/css/biz.css",
                "~/css/fonts.css",
                "~/css/UserManagement.css",
                "~/css/Uploader.css",
                "~/css/ems.css" ) );

            //JQUERY ALERT CSS
            bundles.Add( new StyleBundle( "~/Content/jqalertcss" ).Include(
            "~/Content/jquery-confirm.css",
            "~/Content/jquery-confirm.less" ) );
            //JQUERY ALERT JS
            bundles.Add( new ScriptBundle( "~/bundles/jqalert" ).Include(
               "~/Scripts/jquery-confirm.js" ) );

            //************************
            //Design CSS Fullcalendar*
            //************************
            bundles.Add( new StyleBundle( "~/Content/fullcalendar" ).Include(
               "~/fullcalendar/fullcalendar.css",
               "~/fullcalendar/fullcalendar.print.css" ) );
            //************************
            //JS     CSS Fullcalendar*
            //************************
            bundles.Add( new ScriptBundle( "~/bundles/fullcalendar" ).Include(
                "~/fullcalendar/lib/moment.min.js",
                "~/fullcalendar/fullcalendar.min.js" ) );

            bundles.Add( new ScriptBundle( "~/bundles/schedulecontroller" ).Include(
                "~/Scripts/pageJS/CalendarControlEvents.js",
               "~/Scripts/pageJS/CalendarSetup.js"  ) );
        }
    }
}
