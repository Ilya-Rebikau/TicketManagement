using System.Threading.Tasks;
using TicketManagement.PurchaseFlowAPI.Models;

namespace TicketManagement.PurchaseFlowAPI.Interfaces
{
    /// <summary>
    /// Service for purchase flow.
    /// </summary>
    public interface IPurchaseService
    {
        /// <summary>
        /// Get account view model for personal account.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <returns>Account view model.</returns>
        Task<AccountViewModel> GetAccountViewModelForPersonalAccount(string token);

        /// <summary>
        /// Get ticket view model for buy method.
        /// </summary>
        /// <param name="eventSeatId">EventSeat id.</param>
        /// <param name="price">Price for ticket.</param>
        /// <param name="token">Jwt token.</param>
        /// <returns>Ticket view model.</returns>
        Task<TicketViewModel> GetTicketViewModelForBuyAsync(int eventSeatId, double price, string token);

        /// <summary>
        /// Update event seat state to occupied after buying ticket.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <param name="ticketVm">Ticket view model.</param>
        /// <returns>True if state was changed and false if not because of low balance.</returns>
        Task<bool> UpdateEventSeatStateAfterBuyingTicket(string token, TicketViewModel ticketVm);
    }
}
