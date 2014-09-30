namespace Bundling.Extensions.Transforms
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Hosting;
    using System.Web.Optimization;

	using Bundling.Extensions.Helpers;

    using Yahoo.Yui.Compressor;

	public class YuiJsMinify : IBundleTransform
	{
		public void Process(BundleContext context, BundleResponse response)
		{
			Assert.ArgumentNotNull(response, "response");
		    var allMinimized = response.Files.All(file => file.IncludedVirtualPath.EndsWith(".min.js"));

            var content = new StringBuilder();

		    if (allMinimized)
		    {
                // Do not compress already minified files
                content.Append(response.Content);
		    }
            else
		    {
                var virtualPathProvider = HostingEnvironment.VirtualPathProvider;

		        foreach (var file in response.Files)
		        {
                    if (virtualPathProvider.FileExists(file.IncludedVirtualPath))
		            {
                        var virtualFile = virtualPathProvider.GetFile(file.IncludedVirtualPath);
		                string fileContents;
                        using (var reader = new StreamReader(virtualFile.Open()))
                        {
                            fileContents = reader.ReadToEnd();
		                }
                        if (file.IncludedVirtualPath.EndsWith(".min.js"))
                        {
                            // Do not compress already minified files
                            content.Append(fileContents);
                        }
                        else
                        {
                            try
                            {
                                content.Append(new JavaScriptCompressor().Compress(fileContents));
                            }
                            catch (Exception e)
                            {
                                content.Append(fileContents);
                            }
                        }
                        content.AppendLine();

		            }
		        }
		    }

			response.Content = content.ToString();
			response.ContentType = "text/javascript";
		}
	}
}
