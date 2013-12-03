namespace Bundling.Extensions.Renderers
{
	using System.Web;

	public class StylesRenderer : BaseRenderer
	{
        private const string DefaultTagFormat = "<link href=\"{0}\" rel=\"stylesheet\"/>";
        private const string OptimizedExtension = ".css";

		public StylesRenderer()
			: base(DefaultTagFormat, OptimizedExtension)
		{
		}
	}
}
