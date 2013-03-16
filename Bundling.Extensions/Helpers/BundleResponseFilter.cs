namespace Bundling.Extensions.Helpers
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.Optimization;

	public class BundleResponseFilter : MemoryStream
	{
		private readonly Stream response;

		private List<string> bundlePaths;

		public BundleResponseFilter(Stream response)
		{
			this.response = response;
		}

		private IEnumerable<string> BundlePaths
		{
			get
			{
				return this.bundlePaths ?? (this.bundlePaths = this.LoadBundlePaths());
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			var html = Encoding.UTF8.GetString(buffer);
			html = this.ReplaceBundleUrls(html);
			buffer = Encoding.UTF8.GetBytes(html);
			this.response.Write(buffer, offset, buffer.Length);
		}

		private string ReplaceBundleUrls(string html)
		{
			foreach (var bundlePath in this.BundlePaths)
			{
				html = Regex.Replace(
					html,
					string.Format("(?<bundle_path>{0})\\?v=(?<content_hash>[^\"\\s/<>']*)", bundlePath),
					m => this.RewriteBundleUrl(m.Groups["bundle_path"].Value, m.Groups["content_hash"].Value));
			}

			return html;
		}

		private string RewriteBundleUrl(string bundlePath, string bundleHash)
		{
			return string.Format("{0}/{1}", bundlePath, bundleHash);
		}

		private List<string> LoadBundlePaths()
		{
			return BundleTable.Bundles.Select(bundle => VirtualPathUtility.ToAbsolute(bundle.Path)).ToList();
		}
	}
}
