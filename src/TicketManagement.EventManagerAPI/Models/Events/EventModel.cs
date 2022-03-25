using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Models.Events
{
    /// <summary>
    /// Event model.
    /// </summary>
    public class EventModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets layout's id.
        /// </summary>
        public int LayoutId { get; set; }

        /// <summary>
        /// Gets or sets time when event starts.
        /// </summary>
        public DateTime TimeStart { get; set; }

        /// <summary>
        /// Gets or sets time when event ends.
        /// </summary>
        public DateTime TimeEnd { get; set; }

        /// <summary>
        /// Gets or sets image URL.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets state for saving third party event in database.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Gets or sets list of EventAreaViewModel objects.
        /// EventAreaViewModel objects represents event areas.
        /// </summary>
        public IList<EventAreaModelInEvent> EventAreas { get; set; }

        /// <summary>
        /// Gets max X coordinate from event areas in event.
        /// If there are no event seats in event area, then will return 0.
        /// </summary>
        public int MaxXCoord
        {
            get
            {
                return EventAreas is null || EventAreas.Count <= 0 ? 0 : EventAreas.Max(x => x.EventArea.CoordX);
            }
        }

        /// <summary>
        /// Gets max Y coordinate from event areas in event.
        /// If there are no event seats in event area, then will return 0.
        /// </summary>
        public int MaxYCoord
        {
            get
            {
                return EventAreas is null || EventAreas.Count <= 0 ? 0 : EventAreas.Max(x => x.EventArea.CoordY);
            }
        }

        /// <summary>
        /// Convert event dto to event view model.
        /// </summary>
        /// <param name="event">Event dto.</param>
        public static implicit operator EventModel(EventDto @event)
        {
            return new EventModel
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                LayoutId = @event.LayoutId,
                TimeStart = @event.TimeStart,
                TimeEnd = @event.TimeEnd,
                ImageUrl = @event.ImageUrl,
            };
        }

        /// <summary>
        /// Convert event view model to event dto.
        /// </summary>
        /// <param name="eventVm">Event view model.</param>
        public static implicit operator EventDto(EventModel eventVm)
        {
            return new EventDto
            {
                Id = eventVm.Id,
                Name = eventVm.Name,
                Description = eventVm.Description,
                LayoutId = eventVm.LayoutId,
                TimeStart = eventVm.TimeStart,
                TimeEnd = eventVm.TimeEnd,
                ImageUrl = eventVm.ImageUrl,
            };
        }

        /// <summary>
        /// Sorting event areas by X and Y coordinates.
        /// </summary>
        /// <returns>Sorted event areas.</returns>
        public List<EventAreaModelInEvent> SortedEventAreas() => EventAreas.OrderBy(x => x.EventArea.CoordX).ThenBy(y => y.EventArea.CoordY).ToList();

        /// <summary>
        /// Check that event area with X and Y coordinate exist.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>True if exists and false if does not.</returns>
        public bool CheckAreaForExist(int x, int y)
        {
            var area = EventAreas.FirstOrDefault(e => e.EventArea.CoordX == x && e.EventArea.CoordY == y);
            if (area is not null && area.EventSeats.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}