using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Models.Events
{
    /// <summary>
    /// Event view model.
    /// </summary>
    public class EventViewModel
    {
        /// <summary>
        /// Gets or sets layout of event.
        /// </summary>
        public LayoutDto Layout { get; set; }

        /// <summary>
        /// Gets or sets event.
        /// </summary>
        public EventDto Event { get; set; }

        /// <summary>
        /// Privately gets or sets list of EventAreaViewModel objects.
        /// EventAreaViewModel objects represents event areas.
        /// </summary>
        public IList<EventAreaViewModel> EventAreas { private get; set; }

        /// <summary>
        /// Gets max X coordinate from event areas in event.
        /// If there are no event seats in event area, then will return 0.
        /// </summary>
        public int MaxXCoord
        {
            get
            {
                return EventAreas.Count > 0 ? EventAreas.Max(x => x.EventArea.CoordX) : 0;
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
                return EventAreas.Count > 0 ? EventAreas.Max(x => x.EventArea.CoordY) : 0;
            }
        }

        /// <summary>
        /// Sorting event areas by X and Y coordinates.
        /// </summary>
        /// <returns>Sorted event areas.</returns>
        public List<EventAreaViewModel> SortedEventAreas() => EventAreas.OrderBy(x => x.EventArea.CoordX).ThenBy(y => y.EventArea.CoordY).ToList();

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