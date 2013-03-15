using Bundling.Extensions.Helpers;
using dotless.Core;
using dotless.Core.Abstractions;
using dotless.Core.Importers;
using dotless.Core.Loggers;
using dotless.Core.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Optimization;

namespace Bundling.Extensions.Transforms
{
    public class LessTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse bundle)
        {
            context.HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();

            var lessParser = new Parser();
            ILessEngine lessEngine = CreateLessEngine(lessParser);

            var content = new StringBuilder();

            var bundleFiles = new List<BundleFile>();

            foreach (var bundleFile in bundle.Files)
            {
                bool foundMinimizedVersion = false;
                bundleFiles.Add(bundleFile);

                if (BundleTable.EnableOptimizations)
                {
                    var ext = Path.GetExtension(bundleFile.VirtualFile.VirtualPath).ToLower();
                    if (ext == ".less")
                    {
                        var minimizedFileName = string.Format("{0}.min.css", bundleFile.VirtualFile.VirtualPath.Substring(0, bundleFile.VirtualFile.VirtualPath.LastIndexOf(ext)));
                        var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
                        if (virtualPathProvider.FileExists(minimizedFileName))
                        {
                            var minimizedFile = virtualPathProvider.GetFile(minimizedFileName);
                            foundMinimizedVersion = true;
                            using (var reader = new StreamReader(minimizedFile.Open()))
                            {
                                content.Append(reader.ReadToEnd());
                                content.AppendLine();

                                bundleFiles.Add(new BundleFile(minimizedFile.VirtualPath, minimizedFile));
                            }
                        }
                    }
                }

                if (!foundMinimizedVersion)
                {
                    SetCurrentFilePath(lessParser, bundleFile.VirtualFile.VirtualPath);

                    using (var reader = new StreamReader(VirtualPathProvider.OpenFile(bundleFile.VirtualFile.VirtualPath)))
                    {
                        content.Append(lessEngine.TransformToCss(reader.ReadToEnd(), bundleFile.VirtualFile.VirtualPath));
                        content.AppendLine();

                        bundleFiles.AddRange(GetFileDependencies(lessParser).Select(f => new BundleFile(f.VirtualPath, f)));
                    }
                }
            }

            if (BundleTable.EnableOptimizations)
            {
                // include imports in bundle files to register cache dependencies
                bundle.Files = bundleFiles.Distinct().ToList();
            }

            bundle.ContentType = "text/css";
            bundle.Content = content.ToString();
        }

        /// <summary>
        /// Creates an instance of LESS engine.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        private ILessEngine CreateLessEngine(Parser lessParser)
        {
            var logger = new AspNetTraceLogger(LogLevel.Debug, new Http());
            return new LessEngine(lessParser, logger, false, false);
        }

        /// <summary>
        /// Gets the file dependencies (@imports) of the LESS file being parsed.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        /// <returns>An array of file references to the dependent file references.</returns>
        private IEnumerable<VirtualFile> GetFileDependencies(Parser lessParser)
        {
            var pathResolver = GetPathResolver(lessParser);

            foreach (var importPath in lessParser.Importer.Imports)
            {
                yield return HostingEnvironment.VirtualPathProvider.GetFile(pathResolver.GetFullPath(importPath));
            }

            lessParser.Importer.Imports.Clear();
        }

        /// <summary>
        /// Returns an <see cref="IPathResolver"/> instance used by the specified LESS lessParser.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        private dotless.Core.Input.IPathResolver GetPathResolver(Parser lessParser)
        {
            var importer = lessParser.Importer as Importer;
            var fileReader = importer.FileReader as Bundling.Extensions.Helpers.VirtualFileReader;

            return fileReader.PathResolver;
        }

        /// <summary>
        /// Informs the LESS parser about the path to the currently processed file. 
        /// This is done by using a custom <see cref="IPathResolver"/> implementation.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        /// <param name="currentFilePath">The path to the currently processed file.</param>
        private void SetCurrentFilePath(Parser lessParser, string currentFilePath)
        {
            var importer = lessParser.Importer as Importer;

            if (importer == null)
                throw new InvalidOperationException("Unexpected dotless importer type.");

            var fileReader = importer.FileReader as VirtualFileReader;

            if (fileReader == null)
            {
                importer.FileReader = new VirtualFileReader(new VirtualPathResolver(currentFilePath));
            }
            else
            {
                var pathResolver = fileReader.PathResolver as VirtualPathResolver;

                if (pathResolver == null)
                {
                    fileReader.PathResolver = new VirtualPathResolver(currentFilePath);
                }
                else
                {
                    pathResolver.CurrentFilePath = currentFilePath;
                }
            }
        }
    }
}
