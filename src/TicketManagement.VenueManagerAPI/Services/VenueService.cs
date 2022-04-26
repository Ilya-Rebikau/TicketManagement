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
        /// <param name="confgiratuon">IConfiguration object.</param>
        public VenueService(IRepository<Venue> repository, IConverter<Venue, VenueDto> converter, IRepository<Layout> layoutRepository,
            IRepository<Event> eventRepository, IRepository<EventArea> eventAreaRepository, IRepository<EventSeat> eventSeatRepository,
            IConfiguration confgiratuon)
            : base(repository, converter, confgiratuon)
        {
            _layoutRepository = layoutRepository;
            _eventRepository = eventRepository;
            _eventAreaRepository = eventAreaRepository;
            _eventSeatRepository = eventSeatRepository;
        }

        public async override Task<VenueDto> CreateAsync(VenueDto obj)
        {
            CheckForUniqueName(obj);
            CheckForStringFileds(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<VenueDto> UpdateAsync(VenueDto obj)
        {
            CheckForUniqueName(obj);
            CheckForStringFileds(obj);
            return await base.UpdateAsync(obj);
        }

        public async override Task<VenueDto> DeleteAsync(VenueDto obj)
        {
            CheckForTickets(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Check that string fields aren't empty or white space.
        /// </summary>
        /// <param name="obj">Venue.</param>
        /// <exception cref="ValidationException">Generates exception in case string fields are empty or white space.</exception>
        private static void CheckForStringFileds(VenueDto obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Description) || string.IsNullOrWhiteSpace(obj.Name) || string.IsNullOrWhiteSpace(obj.Address))
            {
                throw new ValidationException("Fields can't be empty or white space!");
            }
        }

        /// <summary>
        /// Check that venue name is unique.
        /// </summary>
        /// <param name="obj">Venue.</param>
        /// <exception cref="ValidationException">Generates exception in case name is not unique.</exception>
        private void CheckForUniqueName(VenueDto obj)
        {
            var venues = Repository.GetAll();
            var venuesWithSuchName = venues.Where(venue => venue.Name == obj.Name && venue.Id != obj.Id);
            if (venuesWithSuchName.Any())
            {
                throw new ValidationException("One of venues already has such name!");
            }
        }

        /// <summary>
        /// Checking that there are no tickets in this venue.
        /// </summary>
        /// <param name="obj">Deleting venue.</param>
        /// <exception cref="ValidationException">Generates exception in case there are tickets in this venue.</exception>
        private void CheckForTickets(VenueDto obj)
        {
            var occupiedEventSeats = new List<EventSeat>();
            var layouts = _layoutRepository.GetAll();
            var layoutsInVenue = layouts.Where(l => l.VenueId == obj.Id).ToList();
            foreach (var layout in layoutsInVenue)
            {
                var events = _eventRepository.GetAll();
                var eventsInLayout = events.Where(e => e.LayoutId == layout.Id).ToList();
                foreach (var @event in eventsInLayout)
                {
                    var eventAreas = _eventAreaRepository.GetAll();
                    var eventAreasInEvent = eventAreas.Where(a => a.EventId == @event.Id).ToList();
                    foreach (var eventArea in eventAreasInEvent)
                    {
                        var eventSeats = _eventSeatRepository.GetAll();
                        occupiedEventSeats = eventSeats.Where(s => s.EventAreaId == eventArea.Id).Where(s => s.State == (int)PlaceStatus.Occupied).ToList();
                    }
                }
            }

            if (occupiedEventSeats.Any())
            {
                throw new ValidationException("Someone bought tickets in this venue already!");
            }
        }
    }
}
