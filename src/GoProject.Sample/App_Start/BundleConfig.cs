using System.Web;
using System.Web.Optimization;

namespace GoProject.Sample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/goJs").Include("~/Scripts/GoJs/go.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bpmnJs").Include(
                "~/Scripts/GoJs/DrawCommandHandler.js",
                "~/Scripts/GoJs/BPMNClasses.js",
                "~/Scripts/GoJs/BPMN.js"));

            bundles.Add(new StyleBundle("~/Content/goCss").Include(
                "~/Content/themes/base/jquery-ui.min.css",
                "~/Content/GoJs/bpmn.css"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/site.css"));
        }
    }
}
