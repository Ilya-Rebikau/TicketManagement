using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Web.Models.EventAreas;
using TicketManagement.Web.Models.EventSeats;

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
    }
}
