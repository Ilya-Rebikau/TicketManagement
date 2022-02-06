using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Web.Models.Users
{
    /// <summary>
    /// Create user view model.
    /// </summary>
    public class CreateUserViewModel
    {
        /// <summary>
        /// Gets or sets user email.
        /// </summary>
        [Required]
        [Display(Name="Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
