using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketManagement.UserAPI.Models.Account;

namespace TicketManagement.UserAPI.Interfaces
{
    /// <summary>
    /// Web service for account controller.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Add balance to account.
        /// </summary>
        /// <param name="model">AddBalanceViewModel object.</param>
        /// <returns>IdentityResult.</returns>
        Task<IdentityResult> AddBalance(AddBalanceViewModel model);

        /// <summary>
        /// Get AddBalanceViewModel.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>AddBalanceViewModel object.</returns>
        Task<AddBalanceViewModel> GetBalanceViewModel(string id);

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

        /// <summary>
        /// Logout for user in API.
        /// </summary>
        /// <returns>Task.</returns>
        Task Logout();

        /// <summary>
        /// Gets edit account view model for edit httpget method.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>EditAccountViewModel object.</returns>
        Task<EditAccountViewModel> GetEditAccountViewModelForEdit(string id);

        /// <summary>
        /// Update user in edit httppost method.
        /// </summary>
        /// <param name="model">EditAccountViewModel object.</param>
        /// <returns>IdentityResult object.</returns>
        Task<IdentityResult> UpdateUserInEdit(EditAccountViewModel model);
    }
}
