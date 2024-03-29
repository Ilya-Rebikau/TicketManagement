﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventManagerAPI.Infrastructure;
using TicketManagement.EventManagerAPI.Interfaces;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Services
{
    /// <summary>
    /// Service with CRUD operations and validations for event.
    /// </summary>
    internal class EventCrudService : BaseService<Event, EventDto>, IService<EventDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventCrudService"/> class.
        /// </summary>
        /// <param name="repository">EventRepository object.</param>
        /// <param name="converter">Converter object for events.</param>
        /// <param name="eventAreaRepository">EventAreaRepository object.</param>
        /// <param name="eventSeatRepository">EventSeatRepository object.</param>
        /// <param name="configuration">IConfiguration object.</param>
        public EventCrudService(IRepository<Event> repository, IConverter<Event, EventDto> converter,
            IRepository<EventArea> eventAreaRepository, IRepository<EventSeat> eventSeatRepository, IConfiguration configuration)
            : base(repository, converter, configuration)
        {
            EventAreaRepository = eventAreaRepository;
            EventSeatRepository = eventSeatRepository;
        }

        /// <summary>
        /// Gets or sets EventAreaRepository object.
        /// </summary>
        protected IRepository<EventArea> EventAreaRepository { get; set; }

        /// <summary>
        /// Gets or sets EventSeatRepository object.
        /// </summary>
        protected IRepository<EventSeat> EventSeatRepository { get; set; }

        public async override Task<EventDto> CreateAsync(EventDto obj)
        {
            CheckForStringFileds(obj);
            CheckForLayoutId(obj);
            ConvertTimeToUtc(obj);
            CheckForPrices(obj);
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            CheckForSameLayoutInOneTime(obj);
            return await base.CreateAsync(obj);
        }

        public async override Task<EventDto> UpdateAsync(EventDto obj)
        {
            CheckForStringFileds(obj);
            CheckForLayoutId(obj);
            ConvertTimeToUtc(obj);
            CheckForPrices(obj);
            CheckEventForPastTime(obj);
            CheckForTimeBorders(obj);
            CheckForSameLayoutInOneTime(obj);
            return await base.UpdateAsync(obj);
        }

        public async override Task<EventDto> DeleteAsync(EventDto obj)
        {
            CheckForTickets(obj);
            return await base.DeleteAsync(obj);
        }

        /// <summary>
        /// Check that string fields aren't empty or white space.
        /// </summary>
        /// <param name="obj">Event.</param>
        /// <exception cref="ValidationException">Generates exception in case string fields are empty or white space.</exception>
        private static void CheckForStringFileds(EventDto obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Description) || string.IsNullOrWhiteSpace(obj.Name) || string.IsNullOrWhiteSpace(obj.ImageUrl))
            {
                throw new ValidationException("Fields can't be empty or white space!");
            }
        }

        /// <summary>
        /// Check that event layout id is positive.
        /// </summary>
        /// <param name="obj">Event.</param>
        /// <exception cref="ValidationException">Generates exception in case layout id isn't positive.</exception>
        private static void CheckForLayoutId(EventDto obj)
        {
            if (obj.LayoutId <= 0)
            {
                throw new ValidationException("Layout id must be positive");
            }
        }

        /// <summary>
        /// Checking that event's time of end and time of start is not in past.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ValidationException">Generates exception in case TimeStart or TimeEnd in past time.</exception>
        private static void CheckEventForPastTime(EventDto obj)
        {
            if (obj.TimeStart <= DateTime.Now || obj.TimeEnd <= DateTime.Now)
            {
                throw new ValidationException("You can't create event in past!");
            }
        }

        /// <summary>
        /// Checking that event's time of end after time of start.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ValidationException">Generates exception in case TimeStart after TimeEnd.</exception>
        private static void CheckForTimeBorders(EventDto obj)
        {
            if (obj.TimeStart >= obj.TimeEnd)
            {
                throw new ValidationException("Time of start event can't be after event's time of end");
            }
        }

        /// <summary>
        /// Convert time to UTC.
        /// </summary>
        /// <param name="obj">Creating or updating event.</param>
        private static void ConvertTimeToUtc(EventDto obj)
        {
            obj.TimeStart = TimeZoneInfo.ConvertTimeToUtc(obj.TimeStart);
            obj.TimeEnd = TimeZoneInfo.ConvertTimeToUtc(obj.TimeEnd);
        }

        /// <summary>
        /// Checking that there are no tickets in this event.
        /// </summary>
        /// <param name="obj">Deleting event.</param>
        /// <exception cref="ValidationException">Generates exception in case there are tickets in this event.</exception>
        private void CheckForTickets(EventDto obj)
        {
            var eventSeats = new List<EventSeat>();
            var eventAreas = EventAreaRepository.GetAll();
            var eventAreasInEvent = eventAreas.Where(a => a.EventId == obj.Id).ToList();
            foreach (var eventArea in eventAreasInEvent)
            {
                var allEventSeats = EventSeatRepository.GetAll();
                eventSeats = allEventSeats.Where(s => s.EventAreaId == eventArea.Id).Where(s => s.State == (int)PlaceStatus.Occupied).ToList();
            }

            if (eventSeats.Any())
            {
                throw new ValidationException("Someone bought tickets in this event already!");
            }
        }

        /// <summary>
        /// Checking that event can't be created in one time in one layout.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ValidationException">Generates exception in case event in this layout and time already exists.</exception>
        private void CheckForSameLayoutInOneTime(EventDto obj)
        {
            var events = Repository.GetAll();
            var eventsInLayout = events.Where(ev => ev.LayoutId == obj.LayoutId && obj.TimeStart <= ev.TimeStart && obj.TimeEnd >= ev.TimeEnd && ev.Id != obj.Id);
            if (eventsInLayout.Any())
            {
                throw new ValidationException("You can't create event in one time in one layout!");
            }
        }

        /// <summary>
        /// Checking that all event areas have price for this event.
        /// </summary>
        /// <param name="obj">Adding or updating event.</param>
        /// <exception cref="ValidationException">Generates exception in case event areas haven't price.</exception>
        private void CheckForPrices(EventDto obj)
        {
            var eventAreas = EventAreaRepository.GetAll();
            var eventAreasInEvent = eventAreas.Where(a => a.EventId == obj.Id);
            foreach (var eventArea in eventAreasInEvent)
            {
                if (eventArea.Price <= 0)
                {
                    throw new ValidationException("You can't create event without prices in event areas!");
                }
            }
        }
    }
}
