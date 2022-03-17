using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Web.Models.EventAreas;

namespace TicketManagement.Web.Interfaces.HttpClients
{
    public interface IEventManagerClient
    {
        [Get("eventareas/getareas")]
        public Task<IEnumerable<EventAreaViewModel>> GetEventAreaViewModels([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("eventareas/details/{id}")]
        public Task<EventAreaViewModel> Details([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventareas/create")]
        public Task Create([Header("Authorization")] string token, [Body] EventAreaViewModel model, CancellationToken cancellationToken = default);

        [Get("eventareas/edit/{id}")]
        public Task<EventAreaViewModel> GetEventAreaViewModelForEdit([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventareas/edit/{id}")]
        public Task Edit([Header("Authorization")] string token, [Path] int id, [Body] EventAreaViewModel model, CancellationToken cancellationToken = default);

        [Get("eventareas/delete/{id}")]
        public Task<EventAreaViewModel> GetEventAreaViewModelForDelete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);

        [Post("eventareas/delete/{id}")]
        public Task Delete([Header("Authorization")] string token, [Path] int id, CancellationToken cancellationToken = default);
    }
}
