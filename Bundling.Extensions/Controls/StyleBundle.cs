namespace Bundling.Extensions.Controls
{
	using System.Web.UI;

	using Bundling.Extensions.Helpers;
	using Bundling.Extensions.Renderers;

	public class StyleBundle : Control
    {
        public string Path { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
			Assert.ArgumentNotNull(this.Path, "Path");
			
			base.Render(writer);

			writer.Write(new StylesRenderer().Render(this.Path, writer.Indent).ToHtmlString());
        }
    }
}