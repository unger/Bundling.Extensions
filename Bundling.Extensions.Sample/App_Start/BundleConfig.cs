[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(BundlingTest.App_Start.BundleConfig), "RegisterBundles")]

namespace BundlingTest.App_Start
{
	using System.Web.Optimization;
	using System.Web.Routing;

	using Bundling.Extensions;
	using Bundling.Extensions.Transforms;

	public class BundleConfig
    {
        public static void RegisterBundles()
        {
	        var bundles = BundleTable.Bundles;

            var cssBundle = new Bundle("~/bundles/css");
            cssBundle.Include("~/Content/less/bootstrap.less");
            cssBundle.Include("~/css/main.less");
            cssBundle.Transforms.Add(new LessTransform());
            cssBundle.Transforms.Add(new CssInlineImagesTransform());

            var modernizrBundle = new Bundle("~/bundles/modernizr");
            modernizrBundle.Include("~/js/lib/modernizr-2.6.2.js");

            var scriptBundle = new Bundle("~/bundles/js");
            scriptBundle.Include("~/js/lib/jquery-1.9.1.js");
			/*
            scriptBundle.Include("~/js/main.ts");
            scriptBundle.Transforms.Add(new TypeScriptTransformer());
			*/

#if !DEBUG
            var cssMin = new YuiCssMinify();
            var jsMin = new YuiJsMinify();

            cssBundle.Transforms.Add(cssMin);
            modernizrBundle.Transforms.Add(jsMin);
            scriptBundle.Transforms.Add(jsMin);
            BundleTable.EnableOptimizations = true;
#endif

            bundles.Add(cssBundle);
            bundles.Add(modernizrBundle);
            bundles.Add(scriptBundle);

            RouteTable.Routes.AddBundleRoutes();
        }
    }
}