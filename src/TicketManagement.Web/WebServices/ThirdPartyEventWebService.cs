using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Models.Events;

namespace TicketManagement.Web.WebServices
{
    public class ThirdPartyEventWebService : IThirdPartyEventWebService
    {
        /// <summary>
        /// Event service object..
        /// </summary>
        private readonly IService<EventDto> _service;

        /// <summary>
        /// IReaderService object.
        /// </summary>
        private readonly IReaderService<EventDto> _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventWebService"/> class.
        /// </summary>
        /// <param name="reader">IReaderService object.</param>
        /// <param name="service">Event service object.</param>
        public ThirdPartyEventWebService(IReaderService<EventDto> reader, IService<EventDto> service)
        {
            _reader = reader;
            _service = service;
        }

        public async Task<IEnumerable<EventViewModel>> GetEventViewModelsFromJson(IFormFile file)
        {
            var json = "";
            using (var stream = file.OpenReadStream())
            {
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                json = Encoding.Default.GetString(buffer);
            }

            var events = await _reader.GetAllAsync(json);
            var eventsVm = new List<EventViewModel>();
            foreach (var @event in events)
            {
                eventsVm.Add(@event);
            }

            return eventsVm;
        }

        public async Task<IEnumerable<EventViewModel>> SaveToDatabase(IEnumerable<EventViewModel> events)
        {
            var savedEvents = new List<EventViewModel>();
            foreach (var @event in events)
            {
                if (@event.Checked is true)
                {
                    await _service.CreateAsync(@event);
                    EventViewModel eventVm = @event;
                    savedEvents.Add(eventVm);
                }
            }

            return savedEvents;
        }
    }
}
