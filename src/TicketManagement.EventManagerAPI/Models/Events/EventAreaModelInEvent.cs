using System.Collections.Generic;
using System.Linq;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Models.Events
{
    /// <summary>
    /// Event area model in event.
    /// </summary>
    public class EventAreaModelInEvent
    {
        /// <summary>
        /// Gets or sets EventArea.
        /// </summary>
        public EventAreaDto EventArea { get; set; }

        /// <summary>
        /// Gets max X coordinate from event seats in event area.
        /// If there are no event seats in event area, then will return 0.
        /// </summary>
        public int MaxXCoord
        {
            get
            {
                return EventSeats.Count > 0 && EventSeats is not null ? EventSeats.Max(x => x.Row) : 0;
            }
        }

        /// <summary>
        /// Gets max Y coordinate.
        /// If there are no event seats in event area, then will return 0.
        /// </summary>
        public int MaxYCoord
        {
            get
            {
                return EventSeats.Count > 0 && EventSeats is not null ? EventSeats.Max(x => x.Number) : 0;
            }
        }

        /// <summary>
        /// Gets or sets event seats in this event area.
        /// </summary>
        public List<EventSeatDto> EventSeats { get; set; }

        /// <summary>
        /// Checking that event seat with X and Y coordinate in event area exist.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns>True if exists and false if does not.</returns>
        public bool CheckSeatForExist(int x, int y)
        {
            return EventSeats.FirstOrDefault(e => e.Row == x && e.Number == y) is not null;
        }
    }
}
