using System.Threading.Tasks;
using TicketManagement.UserAPI.Models;
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
        /// <returns>Task with register result.</returns>
        Task<RegisterResult> RegisterUser(RegisterViewModel model);

        /// <summary>
        /// Login for user in API.
        /// </summary>
        /// <param name="model">Register view model.</param>
        /// <returns>Task with login result.</returns>
        Task<LoginResult> Login(LoginViewModel model);

        Task Logout();
    }
}
