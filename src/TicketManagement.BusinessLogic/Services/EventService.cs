using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for event.
    /// </summary>
    internal class EventService : BaseService<Event>, IService<Event>
    {
        /// <summary>
        /// SeatRepository object.
        /// </summary>
        private readonly IRepository<Seat> _seatRepository;

        /// <summary>
        /// AreaRepository object.
        /// </summary>
        private readonly IRepository<Area> _areaRepository;

        /// <summary>
        /// LayoutRepository object.
        /// </summary>
        private readonly IRepository<Layout> _layoutRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="repository">EventRepository object.</param>
        /// <param name="seatRepository">SeatRepository object.</param>
        /// <param name="areaRepository">AreaRepository object.</param>
        /// <param name="layoutRepository">LayoutRepository object.</param>
        public EventService(IRepository<Event> repository, IRepository<Seat> seatRepository, IRepository<Area> areaRepository, IRepository<Layout> layoutRepository)
            : base(repository)
        {
            _seatRepository = seatRepository;
            _areaRepository = areaRepository;
            _layoutRepository = layoutRepository;
        }

        public async override Task<Event> CreateAsync(Event obj)
        {
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            await CheckForSameLayoutInOneTime(obj);
            await CheckForSeats(obj);
            return await Repository.CreateAsync(obj);
        }

        public async override Task<Event> UpdateAsync(Event obj)
        {
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            await CheckForSameLayoutInOneTime(obj);
            await CheckForSeats(obj);
            return await Repository.UpdateAsync(obj);
        }

        /// <summary>
        /// Checking that event's time of end and time of start is not in past.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case TimeStart or TimeEnd in past time.</exception>
        private void CheckEventForPastTime(Event obj)
        {
            if (obj.TimeStart <= DateTime.Now || obj.TimeEnd <= DateTime.Now)
            {
                throw new ArgumentException("You can't create event in past!");
            }
        }

        /// <summary>
        /// Checking that event's time of end after time of start.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case TimeStart after TimeEnd.</exception>
        private void CheckForTimeBorders(Event obj)
        {
            if (obj.TimeStart >= obj.TimeEnd)
            {
                throw new ArgumentException("Time of start event can't be after event's time of end");
            }
        }

        /// <summary>
        /// Checking that event can't be created in one time in one layout.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case event in this layout and time already exists.</exception>
        private async Task CheckForSameLayoutInOneTime(Event obj)
        {
            IEnumerable<Event> events = await Repository.GetAllAsync();
            IEnumerable<Event> eventsInLayout = events.Where(ev => ev.LayoutId == obj.LayoutId && obj.TimeStart >= ev.TimeStart && obj.TimeEnd <= ev.TimeEnd && ev.Id != obj.Id);
            if (eventsInLayout.Any())
            {
                throw new ArgumentException("You can't create event in one time in one layout!");
            }
        }

        /// <summary>
        /// Checking that seats in layout exists.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case there are no seats in layout.</exception>
        private async Task CheckForSeats(Event obj)
        {
            IEnumerable<Layout> layouts = await _layoutRepository.GetAllAsync();
            IEnumerable<Area> areas = await _areaRepository.GetAllAsync();
            IEnumerable<Seat> seats = await _seatRepository.GetAllAsync();
            Layout layout = layouts.SingleOrDefault(layout => layout.Id == obj.LayoutId);
            if (layout != null)
            {
                IEnumerable<Area> areasInLayout = areas.Where(area => area.LayoutId == layout.Id);
                if (areasInLayout.Any())
                {
                    IEnumerable<Seat> totalSeats = new List<Seat>();
                    foreach (Area a in areasInLayout)
                    {
                        IEnumerable<Seat> seatsInArea = seats.Where(seat => a.Id == seat.AreaId);
                        totalSeats.Concat(seatsInArea);
                    }

                    if (totalSeats.Any())
                    {
                        throw new ArgumentException("There are no seats in layout.");
                    }
                }
            }
        }
    }
}
