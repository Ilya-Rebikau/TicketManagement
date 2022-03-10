using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserAPI.Models.Account
{
    /// <summary>
    /// Login view model.
    /// </summary>
    public class LoginViewModel
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
        /// Gets or sets state about remember user or no.
        /// </summary>
        [Display(Name = "Remember")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets or sets url.
        /// Url with place after login.
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
