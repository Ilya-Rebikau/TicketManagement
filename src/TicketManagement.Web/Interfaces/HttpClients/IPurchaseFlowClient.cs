using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestEase;
using TicketManagement.Web.Models.Account;
using TicketManagement.Web.Models.Events;
using TicketManagement.Web.Models.Tickets;

namespace TicketManagement.Web.Interfaces.HttpClients
{
    /// <summary>
    /// Client for PurchaseFlowAPI.
    /// </summary>
    public interface IPurchaseFlowClient
    {
        /// <summary>
        /// Key with authorization string in header.
        /// </summary>
        private const string AuthorizationKey = "Authorization";

        [Get("account/personalaccount")]
        public Task<AccountViewModel> GetAccountViewModelForPersonalAccount([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("events/buy")]
        public Task<TicketViewModel> GetTicketViewModelForBuy([Header(AuthorizationKey)] string token, [Query] int eventSeatId, [Query] double price,
            CancellationToken cancellationToken = default);

        [Put("events/buy")]
        public Task UpdateEventSeatStateAfterBuyingTicket([Header(AuthorizationKey)] string token, [Body] TicketViewModel ticketVm,
            CancellationToken cancellationToken = default);
    }
}
