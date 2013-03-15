namespace Bundling.Extensions.Transforms
{
	using System;
	using System.IO;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.Hosting;
	using System.Web.Optimization;

	public class CssInlineImagesTransform : IBundleTransform
    {
		private const string Format = "url(data:image/{0};base64,{1})";

		private static readonly Regex Url = new Regex(@"url\((([^\)]*)\?embed)\)", RegexOptions.Singleline);

		public void Process(BundleContext context, BundleResponse response)
        {
            HttpContext.Current.Response.Cache.SetLastModifiedFromFileDependencies();

            foreach (Match match in Url.Matches(response.Content))
            {
	            var filename = HostingEnvironment.MapPath(match.Groups[2].Value);
				if (filename != null)
				{
					var file = new FileInfo(filename);
					if (file.Exists)
					{
						string dataUri = this.GetDataUri(file);
						response.Content = response.Content.Replace(match.Value, dataUri);
						HttpContext.Current.Response.AddFileDependency(file.FullName);
					}
				}
            }
        }

        private string GetDataUri(FileInfo file)
        {
            byte[] buffer = File.ReadAllBytes(file.FullName);
            string ext = file.Extension.Substring(1);
            return string.Format(Format, ext, Convert.ToBase64String(buffer));
        }
    }
}
