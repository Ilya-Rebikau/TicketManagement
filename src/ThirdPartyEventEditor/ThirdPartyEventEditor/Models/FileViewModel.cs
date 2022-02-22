using System.IO;

namespace ThirdPartyEventEditor.Models
{
    public class FileViewModel
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets path.
        /// </summary>
        public string PathToDirectory { get; set; }

        /// <summary>
        /// Gets or sets content type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets full path with name to file.
        /// </summary>
        public string FullPathWithName => Path.Combine(PathToDirectory, Name);
    }
}