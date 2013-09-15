namespace Bundling.Extensions.Renderers
{
	using System.Web;

	public abstract class BaseRenderer
	{
		protected BaseRenderer(string tagFormat)
		{
			this.DefaultTagFormat = tagFormat;
		}

		private string DefaultTagFormat { get; set; }

		public IHtmlString Render(string path)
		{
			return this.Render(this.DefaultTagFormat, path, 0);
		}

		public IHtmlString Render(string path, int indent)
		{
			return this.Render(this.DefaultTagFormat, path, indent);
		}

		public IHtmlString Render(string tagFormat, string path)
		{
			return this.Render(tagFormat, path, 0);
		}

		public IHtmlString Render(string tagFormat, string path, int indent)
		{
			return AssetRenderer.Render(tagFormat, path, indent);
		}

        public string RenderUrl(string tagFormat, string path)
        {
            return AssetRenderer.RenderUrl(tagFormat, path);
        }
	}
}
