using System.Configuration;
using System.IO;
using System.Web.Hosting;
using ThirdPartyEventEditor.Interfaces;

namespace ThirdPartyEventEditor.App_Start
{
    /// <summary>
    /// Configuration for files in App_Data
    /// </summary>
    internal class FilesConfig : IFilesConfig
    {
        public string FileName => ConfigurationManager.AppSettings["JsonFileName"];

        public string PathToDirectoryWithFile => ConfigurationManager.AppSettings["DirectoryWithJsonFile"];

        public string FullPathToFile => Path.Combine(HostingEnvironment.ApplicationPhysicalPath, PathToDirectoryWithFile, FileName);
    }
}