using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserAPI.Models
{
    /// <summary>
    /// Represent user's model.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets balance.
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// Gets or sets timezone.
        /// </summary>
        public string TimeZone { get; set; }
    }
}
