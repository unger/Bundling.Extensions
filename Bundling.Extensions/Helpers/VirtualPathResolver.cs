namespace Bundling.Extensions.Helpers
{
	using System.Web;
	using System.Web.Hosting;

	using dotless.Core.Input;

	public class VirtualPathResolver : IPathResolver
    {
        private string currentFileDirectory;
        private string currentFilePath;

        public VirtualPathResolver(string currentFilePath)
        {
            this.CurrentFilePath = currentFilePath;
        }

        public string CurrentFilePath
        {
	        get
	        {
		        return this.currentFilePath;
	        }
	        
			set
			{
		        this.currentFilePath = value;
                this.currentFileDirectory = VirtualPathUtility.GetDirectory(value);
            }
        }

        public string GetFullPath(string path)
        {
            if (path[0] == '~')
            {
                return path;
            }

            if (VirtualPathUtility.IsAbsolute(path))
            {
	            return VirtualPathUtility.ToAppRelative(
		            path, HostingEnvironment.IsHosted ? HostingEnvironment.ApplicationVirtualPath : "/");
            }

            return VirtualPathUtility.Combine(this.currentFileDirectory, path);
        }
    }
}
