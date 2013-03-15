using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;

namespace Bundling.Extensions.Transforms
{
    public class CssInlineImagesTransform : IBundleTransform
    {
        private static readonly Regex url = new Regex(@"url\((([^\)]*)\?embed)\)", RegexOptions.Singleline);
        private const string format = "url(data:image/{0};base64,{1})";

        public void Process(BundleContext context, BundleResponse response)
        {
            HttpContext.Current.Response.Cache.SetLastModifiedFromFileDependencies();

            foreach (Match match in url.Matches(response.Content))
            {
                var file = new FileInfo(HostingEnvironment.MapPath(match.Groups[2].Value));
                if (file.Exists)
                {
                    string dataUri = GetDataUri(file);
                    response.Content = response.Content.Replace(match.Value, dataUri);
                    HttpContext.Current.Response.AddFileDependency(file.FullName);
                }
            }
        }

        private string GetDataUri(FileInfo file)
        {
            byte[] buffer = File.ReadAllBytes(file.FullName);
            string ext = file.Extension.Substring(1);
            return string.Format(format, ext, Convert.ToBase64String(buffer));
        }
    }
}
