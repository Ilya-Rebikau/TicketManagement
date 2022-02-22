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
        /// Full path to json file.
        /// </summary>
        string FullPathToJsonFile { get; }
    }
}