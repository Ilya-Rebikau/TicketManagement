using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserAPI.Models.Users
{
    /// <summary>
    /// Change role model.
    /// </summary>
    public class ChangeRoleModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRoleModel"/> class.
        /// </summary>
        public ChangeRoleModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets user email.
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets all roles.
        /// </summary>
        public IList<IdentityRole> AllRoles { get; set; }

        /// <summary>
        /// Gets or sets all user roles.
        /// </summary>
        public IList<string> UserRoles { get; set; }
    }
}
