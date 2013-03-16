using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bundling.Extensions
{
	using System.Web;

	using Bundling.Extensions.Helpers;

	public class BundleRewriteModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.PostReleaseRequestState += this.ContextPostReleaseRequestState;
		}

		private void ContextPostReleaseRequestState(object sender, EventArgs e)
		{
			var httpContext = HttpContext.Current;

			if (httpContext.Response.ContentType == "text/html")
			{
				httpContext.Response.Filter = new BundleResponseFilter(httpContext.Response.Filter);
			}
		}

		public void Dispose()
		{
			
		}
	}
}
