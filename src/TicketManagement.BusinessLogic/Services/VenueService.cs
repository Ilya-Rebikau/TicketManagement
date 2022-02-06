using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for venue.
    /// </summary>
    internal class VenueService : BaseService<Venue, VenueDto>, IService<VenueDto>
    {
        /// <summary>
        /// LayoutRepository object.
        /// </summary>
        private readonly IRepository<Layout> _layoutRepository;

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
        /// Initializes a new instance of the <see cref="VenueService"/> class.
        /// </summary>
        /// <param name="repository">VenueRepository object.</param>
        /// <param name="converter">Converter object.</param>
        /// <param name="layoutRepository">LayoutRepository object.</param>
        /// <param name="eventRepository">EventRepository object.</param>
        /// <param name="eventAreaRepository">EventAreaRepository object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        public VenueService(IRepository<Venue> repository, IConverter<Venue, VenueDto> converter, IRepository<Layout> layoutRepository,
            IRepository<Event> eventRepository, IRepository<EventArea> eventAreaRepository, IRepository<EventSeat> eventSeatRepository)
            : base(repository, converter)
        {
            _layoutRepository = layoutRepository;
            _eventRepository = eventRepository;
            _eventAreaRepository = eventAreaRepository;
            _eventSeatRepository = eventSeatRepository;
        }

        public async override Task<VenueDto> DeleteAsync(VenueDto obj)
        {
            await CheckForTickets(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Checking that there are no tickets in this venue.
        /// </summary>
        /// <param name="obj">Deleting venue.</param>
        /// <returns>Task.</returns>
        /// <exception cref="InvalidOperationException">Generates exception in case there are tickets in this venue.</exception>
        private async Task CheckForTickets(VenueDto obj)
        {
            IEnumerable<EventSeat> occupiedEventSeats = new List<EventSeat>();
            var layouts = await _layoutRepository.GetAllAsync();
            var layoutsInVenue = layouts.Where(l => l.VenueId == obj.Id).ToList();
            foreach (var layout in layoutsInVenue)
            {
                var events = await _eventRepository.GetAllAsync();
                var eventsInLayout = events.Where(e => e.LayoutId == layout.Id).ToList();
                foreach (var @event in eventsInLayout)
                {
                    var eventAreas = await _eventAreaRepository.GetAllAsync();
                    var eventAreasInEvent = eventAreas.Where(a => a.EventId == @event.Id).ToList();
                    foreach (var eventArea in eventAreasInEvent)
                    {
                        var eventSeats = await _eventSeatRepository.GetAllAsync();
                        occupiedEventSeats = eventSeats.Where(s => s.EventAreaId == eventArea.Id).Where(s => s.State == (int)PlaceStatus.Occupied).ToList();
                    }
                }
            }

            if (occupiedEventSeats.Any())
            {
                throw new InvalidOperationException("Someone bought tickets in this venue already!");
            }
        }
    }
}
