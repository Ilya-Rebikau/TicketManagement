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
        /// IReaderService object.
        /// </summary>
        private readonly IReaderService<EventDto> _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventWebService"/> class.
        /// </summary>
        /// <param name="reader">IReaderService object.</param>
        public ThirdPartyEventWebService(IReaderService<EventDto> reader)
        {
            _reader = reader;
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
    }
}
