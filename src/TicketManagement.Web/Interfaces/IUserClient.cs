using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

        [Post("account/logout")]
        public Task Logout([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("account/edit/{id}")]
        public Task<EditAccountViewModel> Edit([Header("Authorization")] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Post("account/edit")]
        public Task<IdentityResult> Edit([Header("Authorization")] string token, [Body] EditAccountViewModel model, CancellationToken cancellationToken = default);
    }
}
