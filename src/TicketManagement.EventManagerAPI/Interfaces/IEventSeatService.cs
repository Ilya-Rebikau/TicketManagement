using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Interfaces
{
    /// <summary>
    /// Service for event seats.
    /// </summary>
    public interface IEventSeatService : IService<EventSeatDto>
    {
        /// <summary>
        /// Get free event seats by event id.
        /// </summary>
        /// <param name="eventId">Event id.</param>
        /// <returns>Event seats by event.</returns>
        Task<IEnumerable<EventSeatDto>> GetFreeEventSeatsByEvent(int eventId);
    }
}
