using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Bundling.Extensions.Handlers
{
    public class BundleRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var jsHandler = new BundleHttpHandler();
            requestContext.HttpContext.Items["RouteData"] = requestContext.RouteData;

            return jsHandler;
        }
    }
}
