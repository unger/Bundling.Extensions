using dotless.Core.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Bundling.Extensions.Helpers
{
    public class VirtualFileReader : IFileReader
    {
        public IPathResolver PathResolver { get; set; }

        public VirtualFileReader(IPathResolver pathResolver)
        {
            PathResolver = pathResolver;
        }

        /// <summary>
        /// Returns the binary contents of the specified file.
        /// </summary>
        /// <param name="fileName">The relative, absolute or virtual file path.</param>
        /// <returns>The contents of the specified file as a binary array.</returns>
        public byte[] GetBinaryFileContents(string fileName)
        {
            fileName = PathResolver.GetFullPath(fileName);

            var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
            var virtualFile = virtualPathProvider.GetFile(fileName);
            using (var stream = virtualFile.Open())
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                return buffer;
            }
        }

        /// <summary>
        /// Returns the string contents of the specified file.
        /// </summary>
        /// <param name="fileName">The relative, absolute or virtual file path.</param>
        /// <returns>The contents of the specified file as string.</returns>
        public string GetFileContents(string fileName)
        {
            fileName = PathResolver.GetFullPath(fileName);

            var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
            var virtualFile = virtualPathProvider.GetFile(fileName);
            using (var streamReader = new StreamReader(virtualFile.Open()))
            {
                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Returns a value that indicates if the specified file exists.
        /// </summary>
        /// <param name="fileName">The relative, absolute or virtual file path.</param>
        /// <returns>True if the file exists, otherwise false.</returns>
        public bool DoesFileExist(string fileName)
        {
            fileName = PathResolver.GetFullPath(fileName);

            var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
            return virtualPathProvider.FileExists(fileName);
        }

        public bool UseCacheDependencies { get { return false; } }
    }
}
