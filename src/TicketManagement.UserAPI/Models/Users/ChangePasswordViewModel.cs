using System.ComponentModel.DataAnnotations;

namespace TicketManagement.UserAPI.Models.Users
{
    /// <summary>
    /// Change password for user view model.
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets user email.
        /// </summary>
        [Display(Name ="Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets new password for user.
        /// </summary>
        [Required(ErrorMessage = "FieldRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string NewPassword { get; set; }
    }
}
