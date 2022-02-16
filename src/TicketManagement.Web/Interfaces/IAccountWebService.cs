using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Account;

namespace TicketManagement.Web.Interfaces
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

        /// <summary>
        /// Get account view model for index method.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>Account view model.</returns>
        Task<AccountViewModel> GetAccountViewModelInIndex(User user);
    }
}
