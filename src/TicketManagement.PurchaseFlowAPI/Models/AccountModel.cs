using System.Collections.Generic;

namespace TicketManagement.PurchaseFlowAPI.Models
{
    /// <summary>
    /// Account model.
    /// </summary>
    public class AccountModel
    {
        /// <summary>
        /// Gets or sets jwt token.
        /// </summary>
        public string JwtToken { get; set; }

        /// <summary>
        /// Gets or sets list of AccountTicketViewModel objects.
        /// Represent tickets.
        /// </summary>
        public IList<AccountTicketModel> Tickets { get; set; }
    }
}
