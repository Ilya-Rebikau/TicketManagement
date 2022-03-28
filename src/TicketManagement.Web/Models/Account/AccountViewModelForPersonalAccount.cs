using System.Collections.Generic;

namespace TicketManagement.Web.Models.Account
{
    /// <summary>
    /// Account view model for personal account.
    /// </summary>
    public class AccountViewModelForPersonalAccount
    {
        /// <summary>
        /// Gets or sets User.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets list of AccountTicketViewModel objects.
        /// Represent tickets.
        /// </summary>
        public IList<AccountTicketViewModel> Tickets { get; set; }
    }
}
