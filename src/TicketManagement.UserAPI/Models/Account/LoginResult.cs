﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserAPI.Models.Account
{
    public class LoginResult
    {
        public User User { get; set; }

        public IList<string> Roles { get; set; }

        public SignInResult SignInResult { get; set; }
    }
}
