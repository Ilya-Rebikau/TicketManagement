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
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets user password.
        /// </summary>
        public string Password { get; set; }
    }
}
