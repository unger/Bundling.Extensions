using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace Bundling.Extensions.Handlers
{
    public class BundleHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            RouteData routeData = context.Items["RouteData"] as RouteData;

            var timestamp = routeData.Values["timestamp"].ToString();
            var filepath = (routeData.Values["filepath"] ?? string.Empty).ToString();

            if (string.IsNullOrEmpty(filepath))
            {
                var contextBase = (HttpContextBase)new HttpContextWrapper(context);
                var bundleUrl = VirtualPathUtility.ToAppRelative(context.Request.Url.AbsolutePath.Substring(0, context.Request.Url.AbsolutePath.IndexOf(timestamp) - 1));

                Bundle bundle = BundleTable.Bundles.GetBundleFor(bundleUrl);
                BundleContext bundleContext = new BundleContext(contextBase, BundleTable.Bundles, bundle.Path);
                BundleResponse bundleResponse = this.GetBundleResponse(bundle, bundleContext);
                this.SetBundleHeaders(bundleResponse, bundleContext);
                context.Response.Write(bundleResponse.Content);
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("JsHandler:" + routeData.Values["filepath"]);
            }
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

        private void SetBundleHeaders(BundleResponse bundle, BundleContext context)
        {
            if (context.HttpContext.Response == null)
                return;
            if (bundle.ContentType != null)
                context.HttpContext.Response.ContentType = bundle.ContentType;
            if (context.EnableInstrumentation || context.HttpContext.Response.Cache == null)
                return;
            HttpCachePolicyBase cache = context.HttpContext.Response.Cache;
            cache.SetCacheability(bundle.Cacheability);
            cache.SetOmitVaryStar(true);
            cache.SetExpires(DateTime.Now.AddYears(1));
            cache.SetValidUntilExpires(true);
            cache.SetLastModified(DateTime.Now);
            cache.VaryByHeaders["User-Agent"] = true;
        }


    }
}
