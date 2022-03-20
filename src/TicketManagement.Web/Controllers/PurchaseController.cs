using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models.Account;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for purchase flow.
    /// </summary>
    public class PurchaseController : Controller
    {
        /// <summary>
        /// IPurchaseClient object.
        /// </summary>
        private readonly IPurchaseClient _purchaseClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseController"/> class.
        /// </summary>
        /// <param name="purchaseClient">IPurchaseClient object.</param>
        public PurchaseController(IPurchaseClient purchaseClient)
        {
            _purchaseClient = purchaseClient;
        }

        /// <summary>
        /// Get account view model for personal account.
        /// </summary>
        /// <returns>Account view model.</returns>
        public async Task<AccountViewModel> GetAccountViewModelForPersonalAccount()
        {
            return await _purchaseClient.GetAccountViewModelForPersonalAccount(HttpContext.GetJwtToken());
        }
    }
}
