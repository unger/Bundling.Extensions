namespace Bundling.Extensions.Renderers
{
	using System.Web;

	public abstract class BaseRenderer
	{
		protected BaseRenderer(string tagFormat, string optimizedExtension)
		{
            this.DefaultTagFormat = tagFormat;
            this.OptimizedExtension = optimizedExtension;
        }

		private string DefaultTagFormat { get; set; }

        private string OptimizedExtension { get; set; }

		public IHtmlString Render(string path)
		{
			return this.Render(this.DefaultTagFormat, this.OptimizedExtension, path, 0);
		}

		public IHtmlString Render(string path, int indent)
		{
            return this.Render(this.DefaultTagFormat, this.OptimizedExtension, path, indent);
		}

		public IHtmlString Render(string tagFormat, string path)
		{
            return this.Render(tagFormat, this.OptimizedExtension, path, 0);
		}

		public IHtmlString Render(string tagFormat, string optimizedExtension, string path, int indent)
		{
            return AssetRenderer.Render(tagFormat, optimizedExtension, path, indent);
		}

        public string RenderUrl(string path)
        {
            return AssetRenderer.RenderUrl(path, this.OptimizedExtension);
        }

        public string RenderUrl(string path, string optimizedExtension)
        {
            return AssetRenderer.RenderUrl(path, optimizedExtension);
        }
    }
}
