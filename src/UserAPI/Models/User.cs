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
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets surname.
        /// </summary>
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets balance.
        /// </summary>
        [Range(0, double.MaxValue)]
        [Display(Name = "Balance")]
        public double Balance { get; set; }

        /// <summary>
        /// Gets or sets timezone.
        /// </summary>
        [Display(Name = "TimeZone")]
        public string TimeZone { get; set; }
    }
}
