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
    /// Service with CRUD operations and validations for layout.
    /// </summary>
    internal class LayoutService : BaseService<Layout, LayoutDto>, IService<LayoutDto>
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<EventArea> _eventAreaRepository;
        private readonly IRepository<EventSeat> _eventSeatRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutService"/> class.
        /// </summary>
        /// <param name="repository">LayoutRepository object.</param>
        /// <param name="converter">Converter object.</param>
        /// <param name="eventRepository">EventRepository object.</param>
        /// <param name="eventAreaRepository">EventAreaRepository object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        public LayoutService(IRepository<Layout> repository, IConverter<Layout, LayoutDto> converter,
            IRepository<Event> eventRepository, IRepository<EventArea> eventAreaRepository, IRepository<EventSeat> eventSeatRepository)
            : base(repository, converter)
        {
            _eventRepository = eventRepository;
            _eventAreaRepository = eventAreaRepository;
            _eventSeatRepository = eventSeatRepository;
        }

        public async override Task<LayoutDto> CreateAsync(LayoutDto obj)
        {
            await CheckForUniqueNameInVenue(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<LayoutDto> UpdateAsync(LayoutDto obj)
        {
            await CheckForUniqueNameInVenue(obj);
            return await base.UpdateAsync(obj);
        }

        public async override Task<LayoutDto> DeleteAsync(LayoutDto obj)
        {
            await CheckForTickets(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Checking that all layouts in venue have unique name.
        /// </summary>
        /// <param name="obj">Adding or updating layout.</param>
        /// <exception cref="ArgumentException">Generates exception in case there are layouts in venue with such name.</exception>
        private async Task CheckForUniqueNameInVenue(LayoutDto obj)
        {
            IEnumerable<LayoutDto> layouts = await Converter.ConvertModelsRangeToDtos(await Repository.GetAllAsync());
            IEnumerable<LayoutDto> layoutsInVenue = layouts.Where(layout => layout.Name == obj.Name && layout.VenueId == obj.VenueId && layout.Id != obj.Id);
            if (layoutsInVenue.Any())
            {
                throw new ArgumentException("One of layouts in this venue already has such name!");
            }
        }

        private async Task CheckForTickets(LayoutDto obj)
        {
            IEnumerable<EventSeat> eventSeats = new List<EventSeat>();
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
                throw new InvalidOperationException("Someone bought tickets in this layout already!");
            }
        }
    }
}