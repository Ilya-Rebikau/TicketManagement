using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserAPI.Models.Account
{
    public class RegisterResult
    {
        public IdentityResult IdentityResult { get; set; }

        public User User { get; set; }

        public IList<string> Roles { get; set; }
    }
}
