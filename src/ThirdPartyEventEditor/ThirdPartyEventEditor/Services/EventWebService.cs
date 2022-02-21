using System;
using System.IO;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Services
{
    /// <summary>
    /// Web service for events.
    /// </summary>
    internal class EventWebService : BaseService<ThirdPartyEvent>, IEventWebService
    {
        /// <summary>
        /// IFilesConfig object.
        /// </summary>
        private readonly IFilesConfig _filesConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventWebService"/> class.
        /// </summary>
        /// <param name="repository">IRepository object with CRUD operations.</param>
        /// <param name="filesConfig">IFilesConfig object.</param>
        public EventWebService(IRepository<ThirdPartyEvent> repository, IFilesConfig filesConfig)
            : base(repository)
        {
            Repository = repository;
            _filesConfig = filesConfig;
        }

        /// <summary>
        /// Converting image to base64 string by file name.
        /// </summary>
        /// <param name="imageFileName">Image file name.</param>
        /// <returns>Base64 string image.</returns>
        public async Task<string> UploadSampleImage(string imageFileName)
        {
            var path = Path.Combine(_filesConfig.PathToDirectoryWithFile, imageFileName);
            using (var memoryStream = new MemoryStream())
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
                return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}