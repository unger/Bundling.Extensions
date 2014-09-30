using BundlingTest;

using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(BundleConfig), "RegisterBundles")]

namespace BundlingTest
{
	using System.Web.Optimization;
	using System.Web.Routing;

	using Bundling.Extensions;
	using Bundling.Extensions.Transforms;

	public class BundleConfig
	{
		public static void RegisterBundles()
		{
			BundleCollection bundles = BundleTable.Bundles;

			bundles.UseCdn = true;

			var cssBundle = new Bundle("~/bundles/css");
            cssBundle.Include("~/Content/bootstrap/bootstrap.less");
            cssBundle.Include("~/Content/main.less");
            cssBundle.Include("~/Content/bootstrap/theme.less");
            cssBundle.Transforms.Add(new LessTransform());
			cssBundle.Transforms.Add(new CssInlineImagesTransform());

			var modernizrBundle = new Bundle("~/bundles/modernizr");
            modernizrBundle.Include("~/Scripts/modernizr-2.8.3.js");

			var scriptBundle = new Bundle("~/bundles/js");
            scriptBundle.Include("~/Scripts/jquery-2.1.1.js");
            scriptBundle.Include("~/Scripts/bootstrap.js");
            scriptBundle.Include("~/Scripts/holder.js");

#if !DEBUG
			var cssMin = new YuiCssMinify();
			var scriptMin = new YuiJsMinify();

			cssBundle.Transforms.Add(cssMin);
			modernizrBundle.Transforms.Add(scriptMin);
			scriptBundle.Transforms.Add(scriptMin);
			BundleTable.EnableOptimizations = true;
#endif

			bundles.Add(cssBundle);
			bundles.Add(modernizrBundle);
			bundles.Add(scriptBundle);

			RouteTable.Routes.AddBundleRoutes();
		}
	}
}