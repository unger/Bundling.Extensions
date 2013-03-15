namespace Bundling.Extensions.Transforms
{
	using System.Web.Optimization;

	using Bundling.Extensions.Helpers;

	using Yahoo.Yui.Compressor;

	public class YuiCssMinify : IBundleTransform
	{
		public void Process(BundleContext context, BundleResponse response)
		{
			Assert.ArgumentNotNull(response, "response");

			response.Content = new CssCompressor().Compress(response.Content);
			response.ContentType = "text/css";
		}
	}
}
