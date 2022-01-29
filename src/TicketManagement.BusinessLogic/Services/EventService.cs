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
    /// Service with CRUD operations and validations for event.
    /// </summary>
    internal class EventService : BaseService<Event, EventDto>, IService<EventDto>
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
        /// Converter for seats.
        /// </summary>
        private readonly IConverter<Seat, SeatDto> _seatConverter;

        /// <summary>
        /// Converter for areas.
        /// </summary>
        private readonly IConverter<Area, AreaDto> _areaConverter;

        /// <summary>
        /// Converter for layouts.
        /// </summary>
        private readonly IConverter<Layout, LayoutDto> _layoutConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="repository">EventRepository object.</param>
        /// <param name="seatRepository">SeatRepository object.</param>
        /// <param name="areaRepository">AreaRepository object.</param>
        /// <param name="layoutRepository">LayoutRepository object.</param>
        /// <param name="converter">Converter object for events.</param>
        /// <param name="seatConverter">Converter object for seats.</param>
        /// <param name="areaConverter">Converter object for areas.</param>
        /// <param name="layoutConverter">Converter object for layouts.</param>
        public EventService(IRepository<Event> repository, IRepository<Seat> seatRepository, IRepository<Area> areaRepository, IRepository<Layout> layoutRepository,
            IConverter<Event, EventDto> converter, IConverter<Seat, SeatDto> seatConverter, IConverter<Area, AreaDto> areaConverter, IConverter<Layout, LayoutDto> layoutConverter)
            : base(repository, converter)
        {
            _seatRepository = seatRepository;
            _areaRepository = areaRepository;
            _layoutRepository = layoutRepository;
            _seatConverter = seatConverter;
            _areaConverter = areaConverter;
            _layoutConverter = layoutConverter;
        }

        public async override Task<EventDto> CreateAsync(EventDto obj)
        {
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            await CheckForSameLayoutInOneTime(obj);
            await CheckForSeats(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<EventDto> UpdateAsync(EventDto obj)
        {
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            await CheckForSameLayoutInOneTime(obj);
            await CheckForSeats(obj);
            return await base.UpdateAsync(obj);
        }

        /// <summary>
        /// Checking that event's time of end and time of start is not in past.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ArgumentException">Generates exception in case TimeStart or TimeEnd in past time.</exception>
        private void CheckEventForPastTime(EventDto obj)
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
        private void CheckForTimeBorders(EventDto obj)
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
        private async Task CheckForSameLayoutInOneTime(EventDto obj)
        {
            IEnumerable<EventDto> events = await Converter.ConvertModelsRangeToDtos(await Repository.GetAllAsync());
            IEnumerable<EventDto> eventsInLayout = events.Where(ev => ev.LayoutId == obj.LayoutId && obj.TimeStart >= ev.TimeStart && obj.TimeEnd <= ev.TimeEnd && ev.Id != obj.Id);
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
        private async Task CheckForSeats(EventDto obj)
        {
            await Converter.ConvertModelsRangeToDtos(await Repository.GetAllAsync());
            IEnumerable<LayoutDto> layouts = await _layoutConverter.ConvertModelsRangeToDtos(await _layoutRepository.GetAllAsync());
            IEnumerable<AreaDto> areas = await _areaConverter.ConvertModelsRangeToDtos(await _areaRepository.GetAllAsync());
            IEnumerable<SeatDto> seats = await _seatConverter.ConvertModelsRangeToDtos(await _seatRepository.GetAllAsync());
            LayoutDto layout = layouts.SingleOrDefault(layout => layout.Id == obj.LayoutId);
            if (layout != null)
            {
                IEnumerable<AreaDto> areasInLayout = areas.Where(area => area.LayoutId == layout.Id);
                if (areasInLayout.Any())
                {
                    IEnumerable<SeatDto> totalSeats = new List<SeatDto>();
                    foreach (AreaDto a in areasInLayout)
                    {
                        IEnumerable<SeatDto> seatsInArea = seats.Where(seat => a.Id == seat.AreaId);
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
