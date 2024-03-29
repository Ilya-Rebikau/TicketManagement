﻿using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.Models.Events;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Services
{
    public class ThirdPartyEventService : IThirdPartyEventService
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
        /// IConverter object.
        /// </summary>
        private readonly IConverter<EventDto, EventModel> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventService"/> class.
        /// </summary>
        /// <param name="reader">IReaderService object.</param>
        /// <param name="service">Event service object.</param>
        /// <param name="converter">IConverter object.</param>
        public ThirdPartyEventService(IReaderService<EventDto> reader, IService<EventDto> service, IConverter<EventDto, EventModel> converter)
        {
            _reader = reader;
            _service = service;
            _converter = converter;
        }

        public async Task<IEnumerable<EventModel>> GetEventViewModelsFromJson(byte[] fileData)
        {
            var json = Encoding.Default.GetString(fileData);
            var events = await _reader.GetAllAsync(json);
            return await _converter.ConvertSourceModelRangeToDestinationModelRange(events);
        }

        public async Task<IEnumerable<EventModel>> SaveToDatabase(IEnumerable<EventModel> events)
        {
            var savedEvents = new List<EventModel>();
            foreach (var @event in events)
            {
                if (@event.Checked is true)
                {
                    await _service.CreateAsync(await _converter.ConvertDestinationToSource(@event));
                    EventModel eventVm = @event;
                    savedEvents.Add(eventVm);
                }
            }

            return savedEvents;
        }
    }
}
