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
        public string FileType => ConfigurationManager.AppSettings["FileContentType"];

        public string JsonFileName => ConfigurationManager.AppSettings["JsonFileName"];

        public string PathToAppDataDirectory => HostingEnvironment.MapPath(ConfigurationManager.AppSettings["DirectoryAppData"]);

        public string FullPathToJsonFile => Path.Combine(PathToAppDataDirectory, JsonFileName);
    }
}