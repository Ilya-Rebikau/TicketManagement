using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Models.Events
{
    public class EventViewModel
    {
        public EventDto Event { get; set; }

        public List<EventAreaViewModel> EventAreas { private get; set; }

        public int MaxXCoord
        {
            get
            {
                return EventAreas.Count > 0 ? EventAreas.Max(x => x.EventArea.CoordX) : 0;
            }
        }

        public int MaxYCoord
        {
            get
            {
                return EventAreas.Count > 0 ? EventAreas.Max(x => x.EventArea.CoordY) : 0;
            }
        }

        public List<EventAreaViewModel> SortedEventAreas() => EventAreas.OrderBy(x => x.EventArea.CoordX).ThenBy(y => y.EventArea.CoordY).ToList();

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