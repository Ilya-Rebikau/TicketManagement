﻿using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// Update event.
        /// </summary>
        /// <param name="obj">Updating event.</param>
        /// <returns>Updated event.</returns>
        Task<EventModel> UpdateAsync(EventModel obj);

        /// <summary>
        /// Create event.
        /// </summary>
        /// <param name="obj">Creating event.</param>
        /// <returns>Created event.</returns>
        Task<EventModel> CreateAsync(EventModel obj);

        /// <summary>
        /// Get all event view models.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>Collection of event view models.</returns>
        Task<IEnumerable<EventModel>> GetAllEventViewModelsAsync(int pageNumber);

        /// <summary>
        /// Get event view model for details method.
        /// </summary>
        /// <param name="event">Event dto model for convertations.</param>
        /// <param name="token">Jwt token of user.</param>
        /// <returns>Event view model.</returns>
        Task<EventModel> GetEventViewModelForDetailsAsync(EventDto @event, string token);

        /// <summary>
        /// Get event view model for edit and delete methods.
        /// </summary>
        /// <param name="event">Event.</param>
        /// <param name="token">Jwt token of user.</param>
        /// <returns>Event view model.</returns>
        Task<EventModel> GetEventViewModelForEditAndDeleteAsync(EventDto @event, string token);
    }
}
