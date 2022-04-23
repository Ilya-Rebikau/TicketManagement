using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueManagerAPI.Infrastructure;
using TicketManagement.VenueManagerAPI.Interfaces;
using TicketManagement.VenueManagerAPI.ModelsDTO;

namespace TicketManagement.VenueManagerAPI.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for layout.
    /// </summary>
    internal class LayoutService : BaseService<Layout, LayoutDto>, IService<LayoutDto>
    {
        /// <summary>
        /// EventRepository object.
        /// </summary>
        private readonly IRepository<Event> _eventRepository;

        /// <summary>
        /// EventAreaRepository object.
        /// </summary>
        private readonly IRepository<EventArea> _eventAreaRepository;

        /// <summary>
        /// EventSeatRepository object.
        /// </summary>
        private readonly IRepository<EventSeat> _eventSeatRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutService"/> class.
        /// </summary>
        /// <param name="repository">LayoutRepository object.</param>
        /// <param name="converter">Converter object.</param>
        /// <param name="eventRepository">EventRepository object.</param>
        /// <param name="eventAreaRepository">EventAreaRepository object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        /// <param name="confgiratuon">IConfiguration object.</param>
        public LayoutService(IRepository<Layout> repository, IConverter<Layout, LayoutDto> converter,
            IRepository<Event> eventRepository, IRepository<EventArea> eventAreaRepository, IRepository<EventSeat> eventSeatRepository,
            IConfiguration confgiratuon)
            : base(repository, converter, confgiratuon)
        {
            _eventRepository = eventRepository;
            _eventAreaRepository = eventAreaRepository;
            _eventSeatRepository = eventSeatRepository;
        }

        public async override Task<LayoutDto> CreateAsync(LayoutDto obj)
        {
            CheckForStringFileds(obj);
            CheckForLayoutId(obj);
            await CheckForUniqueNameInVenue(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<LayoutDto> UpdateAsync(LayoutDto obj)
        {
            CheckForStringFileds(obj);
            CheckForLayoutId(obj);
            await CheckForUniqueNameInVenue(obj);
            return await base.UpdateAsync(obj);
        }

        public async override Task<LayoutDto> DeleteAsync(LayoutDto obj)
        {
            await CheckForTickets(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Check that string fields aren't empty or white space.
        /// </summary>
        /// <param name="obj">Layoud.</param>
        /// <exception cref="ValidationException">Generates exception in case string fields are empty or white space.</exception>
        private static void CheckForStringFileds(LayoutDto obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Description) || string.IsNullOrWhiteSpace(obj.Name))
            {
                throw new ValidationException("Fields can't be empty or white space!");
            }
        }

        /// <summary>
        /// Check that venue id is positive.
        /// </summary>
        /// <param name="obj">Layout.</param>
        /// <exception cref="ValidationException">Generates exception in case venue id isn't positive.</exception>
        private static void CheckForLayoutId(LayoutDto obj)
        {
            if (obj.VenueId <= 0)
            {
                throw new ValidationException("Venue id must be positive");
            }
        }

        /// <summary>
        /// Checking that all layouts in venue have unique name.
        /// </summary>
        /// <param name="obj">Adding or updating layout.</param>
        /// <exception cref="ValidationException">Generates exception in case there are layouts in venue with such name.</exception>
        private async Task CheckForUniqueNameInVenue(LayoutDto obj)
        {
            var layouts = await Repository.GetAllAsync();
            var layoutsInVenue = layouts.Where(layout => layout.Name == obj.Name && layout.VenueId == obj.VenueId && layout.Id != obj.Id);
            if (layoutsInVenue.Any())
            {
                throw new ValidationException("One of layouts in this venue already has such name!");
            }
        }

        /// <summary>
        /// Checking that there are no tickets in this layout.
        /// </summary>
        /// <param name="obj">Deleting layout.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ValidationException">Generates exception in case there are tickets in this layout.</exception>
        private async Task CheckForTickets(LayoutDto obj)
        {
            var eventSeats = new List<EventSeat>();
            var events = await _eventRepository.GetAllAsync();
            var eventsInLayout = events.Where(e => e.LayoutId == obj.Id).ToList();
            foreach (var @event in eventsInLayout)
            {
                var eventAreas = await _eventAreaRepository.GetAllAsync();
                var eventAreasInEvent = eventAreas.Where(a => a.EventId == @event.Id).ToList();
                foreach (var eventArea in eventAreasInEvent)
                {
                    var allEventSeats = await _eventSeatRepository.GetAllAsync();
                    eventSeats = allEventSeats.Where(s => s.EventAreaId == eventArea.Id).Where(s => s.State == (int)PlaceStatus.Occupied).ToList();
                }
            }

            if (eventSeats.Any())
            {
                throw new ValidationException("Someone bought tickets in this layout already!");
            }
        }
    }
}