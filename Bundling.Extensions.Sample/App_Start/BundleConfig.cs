using BundlingTest.App_Start;

using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(BundleConfig), "RegisterBundles")]

namespace BundlingTest.App_Start
{
	using System.Web.Optimization;
	using System.Web.Routing;

	using Bundling.Extensions;
	using Bundling.Extensions.Transforms;

	public class BundleConfig
	{
		#region Public Methods and Operators

		public static void RegisterBundles()
		{
			BundleCollection bundles = BundleTable.Bundles;

			bundles.UseCdn = true;

			var cssBundle = new Bundle("~/bundles/css");
			cssBundle.Include("~/Content/less/bootstrap.less");
			cssBundle.Include("~/css/main.less");
			cssBundle.Transforms.Add(new LessTransform());
			cssBundle.Transforms.Add(new CssInlineImagesTransform());

			var modernizrBundle = new Bundle("~/bundles/modernizr");
			modernizrBundle.Include("~/js/lib/modernizr-2.6.2.js");

			var scriptBundle = new Bundle("~/bundles/js");
			scriptBundle.Include("~/js/lib/jquery-1.9.1.min.js");
			scriptBundle.Include("~/js/lib/bootstrap.min.js");

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

		#endregion
	}
}