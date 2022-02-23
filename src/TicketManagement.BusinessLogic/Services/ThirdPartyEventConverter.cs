using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Converter for third party events and event dtos.
    /// </summary>
    internal class ThirdPartyEventConverter : BaseConverter<ThirdPartyEvent, EventDto>, IConverter<ThirdPartyEvent, EventDto>
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
        /// Path to wwwroot folder.
        /// </summary>
        private readonly string _webRootPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventConverter"/> class.
        /// </summary>
        /// <param name="hostingEnvironment">IHostingEnvironment object.</param>
        public ThirdPartyEventConverter(IHostingEnvironment hostingEnvironment)
        {
            _pathToImagesDirectory = Path.Combine(hostingEnvironment.WebRootPath, ImageFolder);
            _webRootPath = hostingEnvironment.WebRootPath;
        }

        public override async Task<ThirdPartyEvent> ConvertDtoToModel(EventDto dto)
        {
            return await Task.Run(() =>
            {
                string url = dto.ImageUrl;
                url = url[1..];
                var config = new MapperConfiguration(cfg => cfg.CreateMap<EventDto, ThirdPartyEvent>()
                    .ForMember(tpe => tpe.StartDate, options => options.MapFrom(dto => dto.TimeStart))
                    .ForMember(tpe => tpe.EndDate, options => options.MapFrom(dto => dto.TimeEnd))
                    .ForMember(tpe => tpe.PosterImage, options => options.MapFrom(dto =>
                        UploadSampleImage(Path.Combine(_webRootPath, url)))));
                var mapper = new Mapper(config);
                var model = mapper.Map<EventDto, ThirdPartyEvent>(dto);
                return model;
            });
        }

        public override async Task<EventDto> ConvertModelToDto(ThirdPartyEvent model)
        {
            return await Task.Run(() =>
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<ThirdPartyEvent, EventDto>()
                    .ForMember(dto => dto.TimeStart, options => options.MapFrom(tpe => tpe.StartDate))
                    .ForMember(dto => dto.TimeEnd, options => options.MapFrom(tpe => tpe.EndDate))
                    .ForMember(dto => dto.ImageUrl, options => options.MapFrom(tpe =>
                        ConvertBase64ToImageAsync(tpe.PosterImage, _pathToImagesDirectory, tpe.Name + ImageFormat))));
                var mapper = new Mapper(config);
                var dto = mapper.Map<ThirdPartyEvent, EventDto>(model);
                return dto;
            });
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
            base64String = base64String.Substring(base64String.IndexOf(',') + 1);
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            image.Save(Path.Combine(path, fileName));
            image.Dispose();
            return Path.Combine("\\", ImageFolder, fileName);
        }

        /// <summary>
        /// Convert image to base64 string.
        /// </summary>
        /// <param name="path">Path to image.</param>
        /// <returns>Bas64 string image.</returns>
        private string UploadSampleImage(string path)
        {
            using (var memoryStream = new MemoryStream())
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                fileStream.CopyTo(memoryStream);
                return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}
