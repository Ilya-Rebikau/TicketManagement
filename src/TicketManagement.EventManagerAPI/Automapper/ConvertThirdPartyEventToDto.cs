using System.Drawing;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Automapper
{
    /// <summary>
    /// Converter for third party events and event dtos.
    /// </summary>
    internal class ConvertThirdPartyEventToDto : ITypeConverter<ThirdPartyEvent, EventDto>
    {
        /// <summary>
        /// Name of folder with images.
        /// </summary>
        private readonly string _imageFolder;

        /// <summary>
        /// Format of images.
        /// </summary>
        private readonly string _imageFormat;

        /// <summary>
        /// Path to folder with images.
        /// </summary>
        private readonly string _pathToImagesDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertThirdPartyEventToDto"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">IWebHostEnvironment object.</param>
        /// <param name="configuration">IConfiguration object.</param>
        public ConvertThirdPartyEventToDto(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            var parentDirectory = Directory.GetParent(hostingEnvironment.ContentRootPath);
            _imageFolder = configuration.GetValue<string>("ImageFolderName");
            _imageFormat = configuration.GetValue<string>("ImageFormat");
            _pathToImagesDirectory = Path.Combine(parentDirectory.FullName, configuration.GetValue<string>("MvcProjectName"),
                configuration.GetValue<string>("wwwrootFolderName"), _imageFolder);
        }

        public EventDto Convert(ThirdPartyEvent source, EventDto destination, ResolutionContext context)
        {
            if (destination != null)
            {
                destination.TimeStart = source.StartDate;
                destination.TimeEnd = source.EndDate;
                destination.Id = source.Id;
                destination.LayoutId = source.LayoutId;
                destination.Name = source.Name;
                destination.Description = source.Description;
                destination.ImageUrl = ConvertBase64ToImageAsync(source.PosterImage, _pathToImagesDirectory, source.Name + _imageFormat);
                return destination;
            }

            var @event = new EventDto
            {
                TimeStart = source.StartDate,
                TimeEnd = source.EndDate,
                Id = source.Id,
                LayoutId = source.LayoutId,
                Name = source.Name,
                Description = source.Description,
                ImageUrl = ConvertBase64ToImageAsync(source.PosterImage, _pathToImagesDirectory, source.Name + _imageFormat),
            };
            return @event;
        }

        /// <summary>
        /// Convert base64 string to image and save it.
        /// </summary>
        /// <param name="base64String">Base64 string.</param>
        /// <param name="path">Path for saving image.</param>
        /// <param name="fileName">Name of saving image.</param>
        /// <returns>Path where image was saved.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы", Justification = "<Ожидание>")]
        private string ConvertBase64ToImageAsync(string base64String, string path, string fileName)
        {
            base64String = base64String[(base64String.IndexOf(',') + 1)..];
            byte[] imageBytes = System.Convert.FromBase64String(base64String);
            using MemoryStream ms = new (imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            image.Save(Path.Combine(path, fileName));
            image.Dispose();
            return Path.Combine("\\", _imageFolder, fileName);
        }
    }
}
