using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.Web.Models.Roles
{
    /// <summary>
    /// Change role view model.
    /// </summary>
    public class ChangeRoleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRoleViewModel"/> class.
        /// </summary>
        public ChangeRoleViewModel()
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
