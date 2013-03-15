namespace Bundling.Extensions
{
	using System.Web;
	using System.Web.Optimization;
	using System.Web.Routing;

	using Bundling.Extensions.Handlers;

	public static class RouteCollectionExtensions
	{
		public static void AddBundleRoutes(this RouteCollection collection)
		{
			foreach (var bundle in BundleTable.Bundles)
			{
				var route = VirtualPathUtility.ToAbsolute(bundle.Path).Trim(new[] { '/' });

				collection.Add(new Route(route + "/{timestamp}/{*filepath}", new BundleRouteHandler()));
			}
		}
	}
}
