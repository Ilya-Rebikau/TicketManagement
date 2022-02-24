namespace ThirdPartyEventEditor.Interfaces
{
    /// <summary>
    /// Configuration for files.
    /// </summary>
    public interface IFilesConfig
    {
        /// <summary>
        /// Gets content type of file.
        /// </summary>
        string FileType { get; }

        /// <summary>
        /// Gets name of json file.
        /// </summary>
        string JsonFileName { get; }

        /// <summary>
        /// Gets path to App_Data directory with files.
        /// </summary>
        string PathToAppDataDirectory { get; }

        /// <summary>
        /// Gets full path to json file.
        /// </summary>
        string FullPathToJsonFile { get; }

        /// <summary>
        /// Gets name of file with logs.
        /// </summary>
        string LogsFileName { get; }

        /// <summary>
        /// Gets full path with name to logs file.
        /// </summary>

        string FullPathToLogsFile { get; }
    }
}