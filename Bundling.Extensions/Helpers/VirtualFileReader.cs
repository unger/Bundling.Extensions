namespace Bundling.Extensions.Helpers
{
	using System.IO;
	using System.Web.Hosting;

	using dotless.Core.Input;

	public class VirtualFileReader : IFileReader
	{
		public VirtualFileReader(IPathResolver pathResolver)
		{
			this.PathResolver = pathResolver;
		}

		public IPathResolver PathResolver { get; set; }

		public bool UseCacheDependencies
		{
			get
			{
				return false;
			}
		}

		public byte[] GetBinaryFileContents(string fileName)
		{
			fileName = this.PathResolver.GetFullPath(fileName);

			var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
			var virtualFile = virtualPathProvider.GetFile(fileName);
			using (var stream = virtualFile.Open())
			{
				var buffer = new byte[stream.Length];
				stream.Read(buffer, 0, (int)stream.Length);
				return buffer;
			}
		}

		public string GetFileContents(string fileName)
		{
			fileName = this.PathResolver.GetFullPath(fileName);

			var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
			var virtualFile = virtualPathProvider.GetFile(fileName);
			using (var streamReader = new StreamReader(virtualFile.Open()))
			{
				return streamReader.ReadToEnd();
			}
		}

		public bool DoesFileExist(string fileName)
		{
			fileName = this.PathResolver.GetFullPath(fileName);

			var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
			return virtualPathProvider.FileExists(fileName);
		}
	}
}
