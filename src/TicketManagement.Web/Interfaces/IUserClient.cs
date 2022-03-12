using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Web.Models.Account;

namespace TicketManagement.Web.Interfaces
{
    public interface IUserClient
    {
        [Post("account/register")]
        public Task<string> Register([Body] RegisterViewModel userModel, CancellationToken cancellationToken = default);

        [Post("account/login")]
        public Task<string> Login([Body] LoginViewModel userModel, CancellationToken cancellationToken = default);

        [Get("account/validate")]
        public Task<bool> Validate([Body] string token, CancellationToken cancellationToken = default);
    }
}
