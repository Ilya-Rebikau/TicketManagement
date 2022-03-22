﻿namespace TicketManagement.UserAPI.Models.Users
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
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets new password for user.
        /// </summary>
        public string NewPassword { get; set; }
    }
}
