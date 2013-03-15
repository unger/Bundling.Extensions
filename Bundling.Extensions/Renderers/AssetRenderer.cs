namespace Bundling.Extensions.Renderers
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Web;
	using System.Web.Hosting;
	using System.Web.Optimization;

	public static class AssetRenderer
    {
        private static HttpContextBase context;

        internal static HttpContextBase Context
        {
            get
            {
                return context ?? new HttpContextWrapper(HttpContext.Current);
            }

            set
            {
                context = value;
            }
        }

        public static IHtmlString Render(string tagFormat, string path, int indent)
        {
            var bundle = BundleTable.Bundles.FirstOrDefault(b => b.Path == path);
            var stringBuilder = new StringBuilder();

            if (bundle != null)
            {
                var bundleContext = new BundleContext(Context, BundleTable.Bundles, path);

                var bundlehash = GetTimeStamp(bundle, bundleContext);

                if (BundleTable.EnableOptimizations)
                {
                    var bundlepath = VirtualPathUtility.ToAbsolute(string.Format("{0}/{1}", path, bundlehash));
                    stringBuilder.Append(string.Format(tagFormat, bundlepath));
                    stringBuilder.Append(Environment.NewLine);
                }
                else
                {
                    foreach (var file in bundle.EnumerateFiles(bundleContext))
                    {
                        var combinedpath = Path.Combine(path, bundlehash, file.VirtualFile.VirtualPath.Substring(1));

                        combinedpath = VirtualPathUtility.ToAbsolute(combinedpath);

                        stringBuilder.Append(string.Format(tagFormat, combinedpath));
                        stringBuilder.Append(Environment.NewLine);
                    }
                }
            }

            return new HtmlString(stringBuilder.ToString());
        }

        private static string GetTimeStamp(Bundle bundle, BundleContext bundleContext)
        {
            var lastdate = DateTime.MinValue;
            foreach (var file in bundle.EnumerateFiles(bundleContext))
            {
                var mappedPath = HostingEnvironment.MapPath(file.IncludedVirtualPath);
				if (mappedPath != null && File.Exists(mappedPath))
                {
                    var fileDate = File.GetLastWriteTime(mappedPath);
                    if (fileDate > lastdate)
                    {
                        lastdate = fileDate;
                    }
                }
            }

            return lastdate.ToString("yyyyMMddHHmmss");
        }
    }
}
