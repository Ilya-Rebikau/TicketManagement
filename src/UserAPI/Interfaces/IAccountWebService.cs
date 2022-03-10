using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Models.Account;

namespace TicketManagement.UserAPI.Interfaces
{
    /// <summary>
    /// Web service for account controller.
    /// </summary>
    public interface IAccountWebService
    {
        /// <summary>
        /// Register user in system.
        /// </summary>
        /// <param name="model">RegisterViewModel object.</param>
        /// <returns>IdentityResult with success or errors.</returns>
        Task<IdentityResult> RegisterUser(RegisterViewModel model);

        /// <summary>
        /// Update user in edit method.
        /// </summary>
        /// <param name="model">EditAccountViewModel object.</param>
        /// <param name="user">User.</param>
        /// <returns>IdentityResult object.</returns>
        Task<IdentityResult> UpdateUserInEdit(EditAccountViewModel model, User user);

        /// <summary>
        /// Add balance to user.
        /// </summary>
        /// <param name="model">AddBalanceViewModel object.</param>
        /// <returns>IdentityResult object.</returns>
        Task<IdentityResult> AddBalanceToUser(AddBalanceViewModel model);
    }
}
