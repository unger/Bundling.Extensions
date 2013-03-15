﻿namespace Bundling.Extensions.Renderers
{
	using System.Web;

	public class ScriptRenderer : BaseRenderer
	{
		private const string DefaultTagFormat = "<script src=\"{0}\"></script>";

		public ScriptRenderer()
			: base(DefaultTagFormat)
		{
		}
	}
}
