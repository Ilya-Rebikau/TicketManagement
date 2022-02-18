namespace ThirdPartyEventEditor.Interfaces
{
    /// <summary>
    /// Configuration for files.
    /// </summary>
    public interface IFilesConfig
    {
        /// <summary>
        /// Name of file.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Path to directory with files.
        /// </summary>
        string PathToDirectoryWithFile { get; }

        /// <summary>
        /// Full path to json file in folder.
        /// </summary>
        string FullPathToFile { get; }
    }
}