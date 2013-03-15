namespace Bundling.Extensions.Handlers
{
	using System.Web;
	using System.Web.Routing;

	public class BundleRouteHandler : IRouteHandler
	{
		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			var bundleHandler = new BundleHttpHandler();
			requestContext.HttpContext.Items["RouteData"] = requestContext.RouteData;

			return bundleHandler;
		}
	}
}
