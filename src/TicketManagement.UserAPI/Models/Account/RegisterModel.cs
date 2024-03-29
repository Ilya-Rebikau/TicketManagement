﻿namespace TicketManagement.UserAPI.Models.Account
{
    /// <summary>
    /// Register model.
    /// </summary>
    public class RegisterModel
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
        /// Gets or sets password confirmation.
        /// </summary>
        public string PasswordConfirm { get; set; }
    }
}
