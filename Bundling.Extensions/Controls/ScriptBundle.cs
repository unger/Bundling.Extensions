namespace Bundling.Extensions.Controls
{
	using System.Web.UI;

	using Bundling.Extensions.Helpers;
	using Bundling.Extensions.Renderers;

	public class ScriptBundle : Control
	{
		public string Path { get; set; }

		protected override void Render(HtmlTextWriter writer)
		{
			Assert.ArgumentNotNull(this.Path, "Path");

			base.Render(writer);

			writer.Write(new ScriptRenderer().Render(this.Path, writer.Indent).ToHtmlString());
		}
	}
}
