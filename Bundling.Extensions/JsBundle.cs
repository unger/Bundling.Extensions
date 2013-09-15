using Bundling.Extensions.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Optimization;

namespace Bundling.Extensions
{
    public class JsBundle : Bundle
    {
        public JsBundle(string virtualPath)
            : base(virtualPath, null, new IBundleTransform[1]
                  {
                    (IBundleTransform) new JsTransform()
                  })
        {
            this.ConcatenationToken = ";" + Environment.NewLine;
        }
    }
}
