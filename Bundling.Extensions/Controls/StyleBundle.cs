using Bundling.Extensions.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace Bundling.Extensions.Controls
{
    public class StyleBundle : Control
    {
        public string Path { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            writer.Write(StylesRenderer.Render(Path).ToHtmlString());
        }

    }
}