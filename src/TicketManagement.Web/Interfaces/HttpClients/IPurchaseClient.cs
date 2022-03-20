using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Web.Models.Account;

namespace TicketManagement.Web.Interfaces.HttpClients
{
    public interface IPurchaseClient
    {
        [Post("account/personalaccount")]
        public Task<AccountViewModel> GetAccountViewModelForPersonalAccount([Header("Authorization")] string token, CancellationToken cancellationToken = default);
    }
}
