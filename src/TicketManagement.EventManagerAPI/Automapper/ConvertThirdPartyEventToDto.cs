using System.Drawing;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
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
        private const string ImageFolder = "images";

        /// <summary>
        /// Format of images.
        /// </summary>
        private const string ImageFormat = ".png";

        /// <summary>
        /// Path to folder with images.
        /// </summary>
        private readonly string _pathToImagesDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertThirdPartyEventToDto"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">IWebHostEnvironment object.</param>
        public ConvertThirdPartyEventToDto(IWebHostEnvironment hostingEnvironment)
        {
            _pathToImagesDirectory = Path.Combine(hostingEnvironment.WebRootPath, ImageFolder);
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
                destination.ImageUrl = ConvertBase64ToImageAsync(source.PosterImage, _pathToImagesDirectory, source.Name + ImageFormat);
                return destination;
            }

            var @event = new EventDto
            {
                TimeStart = source.StartDate,
                Id = source.Id,
                LayoutId = source.LayoutId,
                Name = source.Name,
                Description = source.Description,
                ImageUrl = ConvertBase64ToImageAsync(source.PosterImage, _pathToImagesDirectory, source.Name + ImageFormat),
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
        private string ConvertBase64ToImageAsync(string base64String, string path, string fileName)
        {
            base64String = base64String[(base64String.IndexOf(',') + 1)..];
            byte[] imageBytes = System.Convert.FromBase64String(base64String);
            using MemoryStream ms = new (imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            image.Save(Path.Combine(path, fileName));
            image.Dispose();
            return Path.Combine("\\", ImageFolder, fileName);
        }
    }
}
