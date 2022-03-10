using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserAPI.Models.Users
{
    /// <summary>
    /// Create user view model.
    /// </summary>
    public class CreateUserViewModel
    {
        /// <summary>
        /// Gets or sets user email.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name="Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
