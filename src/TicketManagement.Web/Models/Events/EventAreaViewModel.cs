using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Models.Events
{
    public class EventAreaViewModel
    {
        public EventAreaDto EventArea { get; set; }

        public int MaxXCoord
        {
            get
            {
                return EventSeats.Count > 0 ? EventSeats.Max(x => x.Row) : 0;
            }
        }

        public int MaxYCoord
        {
            get
            {
                return EventSeats.Count > 0 ? EventSeats.Max(x => x.Number) : 0;
            }
        }

        public List<EventSeatDto> EventSeats { get; set; }

        public bool CheckSeatForExist(int x, int y)
        {
            var seat = EventSeats.FirstOrDefault(e => e.Row == x && e.Number == y);
            if (seat is not null)
            {
                return true;
            }

            return false;
        }
    }
}
