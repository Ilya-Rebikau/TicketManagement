using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Web.Models.EventAreas;
using TicketManagement.Web.Models.Events;
using TicketManagement.Web.Models.EventSeats;
using TicketManagement.Web.Models.ThirdPartyEvents;
using TicketManagement.Web.Models.Tickets;

namespace TicketManagement.Web.Interfaces.HttpClients
{
    public interface IEventManagerClient
    {
        [Get("eventareas/getareas")]
        public Task<IEnumerable<EventAreaViewModel>> GetEventAreaViewModels([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("eventareas/details/{id}")]
        public Task<EventAreaViewModel> EventAreaDetails([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventareas/create")]
        public Task CreateEventArea([Header("Authorization")] string token, [Body] EventAreaViewModel model, CancellationToken cancellationToken = default);

        [Get("eventareas/edit/{id}")]
        public Task<EventAreaViewModel> GetEventAreaViewModelForEdit([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventareas/edit/{id}")]
        public Task EditEventArea([Header("Authorization")] string token, [Path] int id, [Body] EventAreaViewModel model, CancellationToken cancellationToken = default);

        [Get("eventareas/delete/{id}")]
        public Task<EventAreaViewModel> GetEventAreaViewModelForDelete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventareas/delete/{id}")]
        public Task DeleteEventArea([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("eventseats/geteventseats")]
        public Task<IEnumerable<EventSeatViewModel>> GetEventSeatsViewModels([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("eventseats/details/{id}")]
        public Task<EventSeatViewModel> EventSeatDetails([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventseats/create")]
        public Task CreateEventSeat([Header("Authorization")] string token, [Body] EventSeatViewModel model, CancellationToken cancellationToken = default);

        [Get("eventseats/edit/{id}")]
        public Task<EventSeatViewModel> GetEventSeatViewModelForEdit([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventseats/edit/{id}")]
        public Task EditEventSeat([Header("Authorization")] string token, [Path] int id, [Body] EventSeatViewModel model, CancellationToken cancellationToken = default);

        [Get("eventseats/delete/{id}")]
        public Task<EventSeatViewModel> GetEventSeatViewModelForDelete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventseats/delete/{id}")]
        public Task DeleteEventSeat([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("events/getevents")]
        public Task<IEnumerable<EventViewModel>> GetEventViewModels([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("events/details/{id}")]
        public Task<EventViewModel> EventDetails([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("events/create")]
        public Task CreateEvent([Header("Authorization")] string token, [Body] EventViewModel model, CancellationToken cancellationToken = default);

        [Get("events/edit/{id}")]
        public Task<EventViewModel> GetEventViewModelForEdit([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("events/edit/{id}")]
        public Task EditEvent([Header("Authorization")] string token, [Path] int id, [Body] EventViewModel model, CancellationToken cancellationToken = default);

        [Get("events/delete/{id}")]
        public Task<EventViewModel> GetEventViewModelForDelete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("events/delete/{id}")]
        public Task DeleteEvent([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Get("tickets/gettickets")]
        public Task<IEnumerable<TicketViewModel>> GetTicketViewModels([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("tickets/details/{id}")]
        public Task<TicketViewModel> TicketDetails([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("tickets/create")]
        public Task CreateTicket([Header("Authorization")] string token, [Body] TicketViewModel model, CancellationToken cancellationToken = default);

        [Get("tickets/edit/{id}")]
        public Task<TicketViewModel> GetTicketViewModelForEdit([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("tickets/edit/{id}")]
        public Task EditTicket([Header("Authorization")] string token, [Path] int id, [Body] TicketViewModel model, CancellationToken cancellationToken = default);

        [Get("tickets/delete/{id}")]
        public Task<TicketViewModel> GetTicketViewModelForDelete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("tickets/delete/{id}")]
        public Task DeleteTicket([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("thirdpartyevents/getevents")]
        public Task<IEnumerable<EventViewModel>> GetThirdPartyEventViewModels([Header("Authorization")] string token, [Body] ThirdPartyEventData data,
            CancellationToken cancellationToken = default);

        [Post("thirdpartyevents/save")]
        public Task SaveThirdPartyEventsToDatabase([Header("Authorization")] string token, [Body] IEnumerable<EventViewModel> events, CancellationToken cancellationToken = default);
    }
}
