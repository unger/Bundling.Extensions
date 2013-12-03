namespace Bundling.Extensions.Renderers
{
	using System.Web;

	public class ScriptRenderer : BaseRenderer
	{
		private const string DefaultTagFormat = "<script src=\"{0}\"></script>";
        private const string OptimizedExtension = ".js";

		public ScriptRenderer()
            : base(DefaultTagFormat, OptimizedExtension)
		{
		}
	}
}
