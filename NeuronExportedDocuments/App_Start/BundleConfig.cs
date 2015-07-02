using System.Web.Optimization;

namespace NeuronExportedDocuments
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/ClientScripts")
                .Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/moment*",
                "~/Scripts/locale/*.js",
                "~/Scripts/bootstrap*"
                //,"~/Scripts/jquery.validate.unobtrusive.bootstrap.tooltip.js"
                ,"~/Scripts/bootstrap-validation*"
                , "~/Scripts/bootstrap-datetimepicker*"
                ));

            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                "~/Scripts/globalize.js",
                "~/Scripts/globalize/cultures/globalize.culture.ru-RU.js"));

            bundles.Add(new ScriptBundle("~/bundles/globalLastFile").Include(
                "~/Scripts/jquery.validate.globalize*"));
        }
    }
}