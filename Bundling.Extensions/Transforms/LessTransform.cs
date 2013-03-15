namespace Bundling.Extensions.Transforms
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Web.Hosting;
	using System.Web.Optimization;

	using Bundling.Extensions.Helpers;

	using dotless.Core;
	using dotless.Core.Abstractions;
	using dotless.Core.Importers;
	using dotless.Core.Loggers;
	using dotless.Core.Parser;

	public class LessTransform : IBundleTransform
	{
		public void Process(BundleContext context, BundleResponse bundle)
		{
			context.HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();

			var lessParser = new Parser();
			ILessEngine lessEngine = this.CreateLessEngine(lessParser);

			var content = new StringBuilder();

			var bundleFiles = new List<BundleFile>();

			foreach (var bundleFile in bundle.Files)
			{
				bool foundMinimizedVersion = false;
				bundleFiles.Add(bundleFile);

				if (BundleTable.EnableOptimizations)
				{
					var ext = Path.GetExtension(bundleFile.VirtualFile.VirtualPath);
					if (ext != null && ext.Equals(".less", StringComparison.InvariantCultureIgnoreCase))
					{
						var filepath = bundleFile.VirtualFile.VirtualPath;
						var minimizedFileName = string.Format(
							"{0}.min.css",
							filepath.Substring(0, filepath.LastIndexOf(ext, StringComparison.Ordinal)));
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
					this.SetCurrentFilePath(lessParser, bundleFile.VirtualFile.VirtualPath);

					using (var reader = new StreamReader(VirtualPathProvider.OpenFile(bundleFile.VirtualFile.VirtualPath)))
					{
						content.Append(lessEngine.TransformToCss(reader.ReadToEnd(), bundleFile.VirtualFile.VirtualPath));
						content.AppendLine();

						bundleFiles.AddRange(this.GetFileDependencies(lessParser).Select(f => new BundleFile(f.VirtualPath, f)));
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

		private ILessEngine CreateLessEngine(Parser lessParser)
		{
			var logger = new AspNetTraceLogger(LogLevel.Debug, new Http());
			return new LessEngine(lessParser, logger, false, false);
		}

		private IEnumerable<VirtualFile> GetFileDependencies(Parser lessParser)
		{
			var pathResolver = this.GetPathResolver(lessParser);

			foreach (var importPath in lessParser.Importer.Imports)
			{
				yield return HostingEnvironment.VirtualPathProvider.GetFile(pathResolver.GetFullPath(importPath));
			}

			lessParser.Importer.Imports.Clear();
		}

		private dotless.Core.Input.IPathResolver GetPathResolver(Parser lessParser)
		{
			var importer = lessParser.Importer as Importer;
			if (importer != null)
			{
				var fileReader = importer.FileReader as VirtualFileReader;

				if (fileReader != null)
				{
					return fileReader.PathResolver;
				}
			}

			return null;
		}

		private void SetCurrentFilePath(Parser lessParser, string currentFilePath)
		{
			var importer = lessParser.Importer as Importer;

			if (importer == null)
			{
				throw new InvalidOperationException("Unexpected dotless importer type.");
			}

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
