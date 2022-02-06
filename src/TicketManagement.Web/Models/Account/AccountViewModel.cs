using System.Collections.Generic;

namespace TicketManagement.Web.Models.Account
{
    public class AccountViewModel
    {
        public User User { get; set; }

        public IList<AccountTicketViewModel> Tickets { get; set; }
    }
}
