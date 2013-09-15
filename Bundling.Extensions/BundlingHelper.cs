namespace Bundling.Extensions
{
	using System.Web;

	using Bundling.Extensions.Renderers;

	public static class BundlingHelper
	{
        public static IHtmlString RenderScript(string path)
        {
            return new ScriptRenderer().Render(path);
        }

        public static string RenderScriptUrl(string path)
        {
            return new ScriptRenderer().RenderUrl("{0}.js", path);
        }

        public static IHtmlString RenderStyles(string path)
        {
            return new StylesRenderer().Render(path);
        }
 
        public static string RenderStylesUrl(string path)
        {
            return new StylesRenderer().RenderUrl("{0}.css", path);
        }
    }
}
