﻿using Bundling.Extensions.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;

namespace Bundling.Extensions.Renderers
{
    public class ScriptRenderer
    {
        private const string DefaultTagFormat = "<script src=\"{0}\"></script>"; 

        public static IHtmlString Render(string path)
        {
            return Render(DefaultTagFormat, path);
        }

        public static IHtmlString Render(string tagFormat, string path)
        {
            return AssetRenderer.Render(tagFormat, path);
        }
    }
}
