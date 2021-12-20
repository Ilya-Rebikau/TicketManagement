using System;
using System.Collections.Generic;
using System.Linq;
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

        public override Event Create(Event obj)
        {
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            CheckForSameLayoutInOneTime(obj);
            CheckForSeats(obj);
            return Repository.Create(obj);
        }

        public override Event Update(Event obj)
        {
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            CheckForSameLayoutInOneTime(obj);
            CheckForSeats(obj);
            return Repository.Update(obj);
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
            if (obj.TimeStart <= obj.TimeEnd)
            {
                throw new ArgumentException("Time of start event can't be after event's time of end");
            }
        }

        /// <summary>
        /// Checking that event can't be created in one time in one layout.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case event in this layout and time already exists.</exception>
        private void CheckForSameLayoutInOneTime(Event obj)
        {
            IEnumerable<Event> events = Repository.GetAll();
            foreach (Event ev in events)
            {
                if (ev.LayoutId == obj.LayoutId && obj.TimeStart >= ev.TimeStart && obj.TimeEnd <= ev.TimeEnd)
                {
                    throw new ArgumentException("You can't create event in one time in one layout!");
                }
            }
        }

        /// <summary>
        /// Checking that seats in layout exists.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case there are no seats in layout.</exception>
        private void CheckForSeats(Event obj)
        {
            IEnumerable<Layout> layouts = _layoutRepository.GetAll();
            IEnumerable<Area> areas = _areaRepository.GetAll();
            IEnumerable<Seat> seats = _seatRepository.GetAll();
            Layout layout = layouts.SingleOrDefault(layout => layout.Id == obj.LayoutId);
            if (layout != null)
            {
                Area area = areas.SingleOrDefault(area => area.LayoutId == layout.Id);
                if (area != null)
                {
                    IEnumerable<Seat> seatsInArea = seats.Where(seat => area.Id == seat.AreaId);
                    if (seatsInArea.Any())
                    {
                        throw new ArgumentException("There are no seats in layout.");
                    }
                }
            }
        }
    }
}
