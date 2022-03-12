using Microsoft.AspNetCore.Identity;
using TicketManagement.UserAPI.Models;

namespace UserAPI.Models.Account
{
    public class LoginResult
    {
        public User User { get; set; }

        public SignInResult SignInResult { get; set; }
    }
}
