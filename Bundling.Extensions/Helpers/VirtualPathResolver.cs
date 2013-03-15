using dotless.Core.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Bundling.Extensions.Helpers
{
    public class VirtualPathResolver : IPathResolver
    {
        private string currentFileDirectory;
        private string currentFilePath;

        public VirtualPathResolver(string currentFilePath)
        {
            CurrentFilePath = currentFilePath;
        }

        /// <summary>
        /// Gets or sets the path to the currently processed file.
        /// </summary>
        public string CurrentFilePath
        {
            get { return currentFilePath; }
            set
            {
                currentFilePath = value;
                currentFileDirectory = VirtualPathUtility.GetDirectory(value);
            }
        }

        /// <summary>
        /// Returns the virtual path for the specified file <param name="path"/>.
        /// </summary>
        /// <param name="path">The imported file path.</param>
        public string GetFullPath(string path)
        {
            if (path[0] == '~') // a virtual path e.g. ~/assets/style.less
            {
                return path;
            }

            if (VirtualPathUtility.IsAbsolute(path)) // an absolute path e.g. /assets/style.less
            {
                return VirtualPathUtility.ToAppRelative(path,
                    HostingEnvironment.IsHosted ? HostingEnvironment.ApplicationVirtualPath : "/");
            }

            // otherwise, assume relative e.g. style.less or ../../variables.less
            return VirtualPathUtility.Combine(currentFileDirectory, path);
        }
    }
}
