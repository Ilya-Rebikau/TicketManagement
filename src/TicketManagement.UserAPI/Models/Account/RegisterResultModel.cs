using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserAPI.Models.Account
{
    /// <summary>
    /// Register result in app.
    /// </summary>
    public class RegisterResultModel
    {
        /// <summary>
        /// Gets or sets IdentityResult.
        /// </summary>
        public IdentityResult IdentityResult { get; set; }

        /// <summary>
        /// Gets or sets User.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets user roles.
        /// </summary>
        public IList<string> Roles { get; set; }
    }
}
