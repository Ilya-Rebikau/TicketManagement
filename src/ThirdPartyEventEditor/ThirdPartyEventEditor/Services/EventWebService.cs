﻿using System;
using System.IO;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Services
{
    /// <summary>
    /// Web service for events.
    /// </summary>
    internal class EventWebService : EventService, IEventWebService
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

        public FileViewModel GetFileInfo()
        {
            var file = new FileViewModel
            {
                Name = _filesConfig.JsonFileName,
                PathToDirectory = _filesConfig.PathToAppDataDirectory,
                Type = _filesConfig.FileType
            };

            return file;
        }

        public async Task<string> UploadSampleImage(string imageFileName)
        {
            var path = Path.Combine(_filesConfig.PathToAppDataDirectory, imageFileName);
            using (var memoryStream = new MemoryStream())
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
                return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}