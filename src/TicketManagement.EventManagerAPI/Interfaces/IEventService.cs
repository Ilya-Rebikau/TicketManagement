using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TicketManagement.EventManagerAPI.Models.Events;
using TicketManagement.EventManagerAPI.ModelsDTO;

namespace TicketManagement.EventManagerAPI.Interfaces
{
    /// <summary>
    /// Ыervice for event controller.
    /// </summary>
    public interface IEventService : IService<EventDto>
    {
        /// <summary>
        /// Get all event view models.
        /// </summary>
        /// <returns>Collection of event view models.</returns>
        Task<IEnumerable<EventViewModel>> GetAllEventViewModelsAsync();

        /// <summary>
        /// Get event view model for details method.
        /// </summary>
        /// <param name="event">Event dto model for convertations.</param>
        /// <returns>Event view model.</returns>
        Task<EventViewModel> GetEventViewModelForDetailsAsync(EventDto @event);

        /// <summary>
        /// Get event view model for edit and delete methods.
        /// </summary>
        /// <param name="event">Event.</param>
        /// <returns>Event view model.</returns>
        Task<EventViewModel> GetEventViewModelForEditAndDeleteAsync(EventDto @event);
    }
}
