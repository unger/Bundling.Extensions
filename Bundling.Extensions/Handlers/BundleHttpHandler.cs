namespace Bundling.Extensions.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Web;
	using System.Web.Hosting;
	using System.Web.Optimization;
	using System.Web.Routing;

	using Bundling.Extensions.Helpers;

	public class BundleHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var routeData = context.Items["RouteData"] as RouteData;

			Assert.ArgumentNotNull(routeData, "routeData");

	        var timestamp = routeData.Values["timestamp"].ToString();
	        string filepath = (routeData.Values["filepath"] ?? string.Empty).ToString();

	        var contextBase = (HttpContextBase)new HttpContextWrapper(context);
			var bundleUrl = VirtualPathUtility.ToAppRelative(context.Request.Url.AbsolutePath.Substring(0, context.Request.Url.AbsolutePath.IndexOf(timestamp, StringComparison.Ordinal) - 1));
			Bundle bundle = BundleTable.Bundles.GetBundleFor(bundleUrl);
			var bundleContext = new BundleContext(contextBase, BundleTable.Bundles, bundle.Path);
	        BundleResponse bundleResponse = string.IsNullOrEmpty(filepath) ? this.GetBundleResponse(bundle, bundleContext) : this.GetSingleFileBundleResponse(bundle, bundleContext, filepath);

			this.SetBundleHeaders(bundleResponse, bundleContext);
			context.Response.Write(bundleResponse.Content);
		}

		private BundleResponse GetSingleFileBundleResponse(Bundle bundle, BundleContext bundleContext, string filepath)
		{
			var files = bundle.EnumerateFiles(bundleContext);
			var file = files.FirstOrDefault(f => f.VirtualFile.VirtualPath.TrimStart(new[] { '/' }) == filepath);

			if (file == null)
			{
				throw new FileNotFoundException(string.Format("File not found '{0}'", filepath));
			}

			string contents;
			var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
            var virtualFile = virtualPathProvider.GetFile(VirtualPathUtility.ToAppRelative(file.VirtualFile.VirtualPath));
            using (var streamReader = new StreamReader(virtualFile.Open()))
            {
                contents = streamReader.ReadToEnd();
            }

			return bundle.ApplyTransforms(bundleContext, contents, new List<BundleFile> { file });
		}

        private BundleResponse GetBundleResponse(Bundle bundle, BundleContext bundleContext)
        {
            BundleResponse response = bundle.CacheLookup(bundleContext);
            if (response == null || bundleContext.EnableInstrumentation)
            {
                response = bundle.GenerateBundleResponse(bundleContext);
                bundle.UpdateCache(bundleContext, response);
            }

            return response;
        }

        private void SetBundleHeaders(BundleResponse bundleResponse, BundleContext context)
        {
	        if (context.HttpContext.Response == null)
	        {
		        return;
	        }

	        if (bundleResponse.ContentType != null)
	        {
		        context.HttpContext.Response.ContentType = bundleResponse.ContentType;
	        }

	        if (context.EnableInstrumentation || context.HttpContext.Response.Cache == null)
	        {
		        return;
	        }

	        HttpCachePolicyBase cache = context.HttpContext.Response.Cache;
	        cache.SetCacheability(bundleResponse.Cacheability);
            cache.SetOmitVaryStar(true);
            cache.SetExpires(DateTime.Now.AddYears(1));
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(DateTime.Now);
            cache.VaryByHeaders["User-Agent"] = true;
        }
    }
}
