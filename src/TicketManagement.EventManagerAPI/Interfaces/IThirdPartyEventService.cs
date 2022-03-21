using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.EventManagerAPI.Models.Events;

namespace TicketManagement.EventManagerAPI.Interfaces
{
    /// <summary>
    /// Service for third party event controller.
    /// </summary>
    public interface IThirdPartyEventService
    {
        /// <summary>
        /// Get event view models.
        /// </summary>
        /// <param name="fileData">Json file in byte array format.</param>
        /// <returns>Collection of event view models.</returns>
        Task<IEnumerable<EventViewModel>> GetEventViewModelsFromJson(byte[] fileData);

        /// <summary>
        /// Save collection of event view models to database.
        /// </summary>
        /// <param name="events">Collection of events.</param>
        /// <returns>Saved collection of events.</returns>
        Task<IEnumerable<EventViewModel>> SaveToDatabase(IEnumerable<EventViewModel> events);
    }
}
