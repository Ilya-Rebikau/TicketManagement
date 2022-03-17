using System.Threading;
using System.Threading.Tasks;
using RestEase;

namespace TicketManagement.Web.Interfaces.HttpClients
{
    public interface IEventManagerClient
    {
        [Post("test/method")]
        public Task<bool> Method([Header("Authorization")] string token, CancellationToken cancellationToken = default);
    }
}
