using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Interfaces
{
    /// <summary>
    /// Web service for events.
    /// </summary>
    public interface IEventWebService : IService<ThirdPartyEvent>
    {
        /// <summary>
        /// Convert image from png to base64.
        /// </summary>
        /// <param name="imageFileName">Name of image file.</param>
        /// <returns>Base64 string with image.</returns>
        Task<string> UploadSampleImage(string imageFileName);

        /// <summary>
        /// Get info about file.
        /// </summary>
        /// <returns>File view model.</returns>
        FileViewModel GetFileInfo();
    }
}
