using System.Collections.Generic;

namespace TicketManagement.PurchaseFlowAPI.Models
{
    /// <summary>
    /// Account view model.
    /// </summary>
    public class AccountViewModel
    {
        /// <summary>
        /// Gets or sets jwt token.
        /// </summary>
        public string JwtToken { get; set; }

        /// <summary>
        /// Gets or sets list of AccountTicketViewModel objects.
        /// Represent tickets.
        /// </summary>
        public IList<AccountTicketViewModel> Tickets { get; set; }
    }
}
