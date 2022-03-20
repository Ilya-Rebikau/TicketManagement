using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Web.Models.Account;
using TicketManagement.Web.Models.Tickets;

namespace TicketManagement.Web.Interfaces.HttpClients
{
    public interface IPurchaseFlowClient
    {
        [Get("account/personalaccount")]
        public Task<AccountViewModel> GetAccountViewModelForPersonalAccount([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("events/buy")]
        public Task<TicketViewModel> GetTicketViewModelForBuy([Header("Authorization")] string token, [Body] Dictionary<int, double> eventSeatIdAndPrice,
            CancellationToken cancellationToken = default);

        [Post("events/buy")]
        public Task UpdateEventSeatStateAfterBuyingTicket([Header("Authorization")] string token, [Body] TicketViewModel ticketVm,
            CancellationToken cancellationToken = default);
    }
}
