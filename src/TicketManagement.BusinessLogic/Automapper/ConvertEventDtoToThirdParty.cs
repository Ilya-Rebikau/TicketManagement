using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Automapper
{
    /// <summary>
    /// Convert event dtos to third party events.
    /// </summary>
    internal class ConvertEventDtoToThirdParty : ITypeConverter<EventDto, ThirdPartyEvent>
    {
        /// <summary>
        /// Path to wwwroot folder.
        /// </summary>
        private readonly string _webRootPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertEventDtoToThirdParty"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">IHostingEnvironment object.</param>
        public ConvertEventDtoToThirdParty(IHostingEnvironment hostingEnvironment)
        {
            _webRootPath = hostingEnvironment.WebRootPath;
        }

        public ThirdPartyEvent Convert(EventDto source, ThirdPartyEvent destination, ResolutionContext context)
        {
            if (destination != null)
            {
                destination.StartDate = source.TimeStart;
                destination.EndDate = source.TimeEnd;
                destination.Id = source.Id;
                destination.LayoutId = source.LayoutId;
                destination.Name = source.Name;
                destination.Description = source.Description;
                destination.PosterImage = UploadSampleImage(Path.Combine(_webRootPath, source.ImageUrl[1..]));
                return destination;
            }

            var @event = new ThirdPartyEvent
            {
                StartDate = source.TimeStart,
                EndDate = source.TimeEnd,
                Id = source.Id,
                LayoutId = source.LayoutId,
                Name = source.Name,
                Description = source.Description,
                PosterImage = UploadSampleImage(Path.Combine(_webRootPath, source.ImageUrl[1..])),
            };

            return @event;
        }

        /// <summary>
        /// Convert image to base64 string.
        /// </summary>
        /// <param name="path">Path to image.</param>
        /// <returns>Bas64 string image.</returns>
        private string UploadSampleImage(string path)
        {
            using var memoryStream = new MemoryStream();
            using var fileStream = new FileStream(path, FileMode.Open);
            fileStream.CopyTo(memoryStream);
            return "data:image/png;base64," + System.Convert.ToBase64String(memoryStream.ToArray());
        }
    }
}
