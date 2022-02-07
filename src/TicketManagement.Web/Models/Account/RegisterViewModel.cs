using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Web.Models.Account
{
    /// <summary>
    /// Register view model.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets password confirmation.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [Compare("Password", ErrorMessage = "PasswordsDifferent")]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirm")]
        public string PasswordConfirm { get; set; }
    }
}
