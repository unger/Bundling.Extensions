using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;
using Yahoo.Yui.Compressor;

namespace Bundling.Extensions.Transforms
{
    public class YuiJsMinify : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("bundle");
            }

            response.Content = new JavaScriptCompressor().Compress(response.Content);
            response.ContentType = "text/javascript";
        }
    }
}
