using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.UserAPI.Models.Account;
using UserAPI.Models.Account;

namespace TicketManagement.UserAPI.Interfaces
{
    /// <summary>
    /// Web service for account controller.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Register user in system.
        /// </summary>
        /// <param name="model">RegisterViewModel object.</param>
        /// <returns>IdentityResult with success or errors, registered user and his roles.</returns>
        Task<RegisterResult> RegisterUser(RegisterViewModel model);

        Task<LoginResult> Login(LoginViewModel model);
    }
}
