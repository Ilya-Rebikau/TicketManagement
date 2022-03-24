namespace TicketManagement.UserAPI.Models.Account
{
    /// <summary>
    /// Login view model.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets state about remember user or no.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets or sets url.
        /// Url with place after login.
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
