using Bundling.Extensions.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace Bundling.Extensions
{
    public static class RouteCollectionUtility
    {
        public static void AddBundleRoutes(this RouteCollection collection)
        {
            foreach (var bundle in BundleTable.Bundles)
            {
                var route = VirtualPathUtility.ToAbsolute(bundle.Path).Trim(new char[] {'/'});

                collection.Add(new Route(route + "/{timestamp}/{*filepath}", new BundleRouteHandler()));
            }
        }
    }
}
