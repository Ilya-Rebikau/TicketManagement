using System.Collections.Generic;

namespace TicketManagement.Web.Models.Account
{
    /// <summary>
    /// Account view model.
    /// </summary>
    public class AccountViewModel
    {
        /// <summary>
        /// Gets or sets user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets list of AccountTicketViewModel objects.
        /// Represent tickets.
        /// </summary>
        public IList<AccountTicketViewModel> Tickets { get; set; }
    }
}
