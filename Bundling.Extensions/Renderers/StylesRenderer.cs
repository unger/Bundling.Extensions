namespace Bundling.Extensions.Renderers
{
	using System.Web;

	public class StylesRenderer : BaseRenderer
    {
		private const string DefaultTagFormat = "<link href=\"{0}\" rel=\"stylesheet\"/>";

		public StylesRenderer()
			: base(DefaultTagFormat)
		{
		}
    }
}
