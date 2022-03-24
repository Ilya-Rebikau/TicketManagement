using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserAPI.Models.Account
{
    /// <summary>
    /// Login result to app.
    /// </summary>
    public class LoginResultModel
    {
        /// <summary>
        /// Gets or sets User.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets user roles.
        /// </summary>
        public IList<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets SignInResult.
        /// </summary>
        public SignInResult SignInResult { get; set; }
    }
}
