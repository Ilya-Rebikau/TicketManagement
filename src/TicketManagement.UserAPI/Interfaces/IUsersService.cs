using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Models.Users;

namespace TicketManagement.UserAPI.Interfaces
{
    /// <summary>
    /// Service for users controller.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>All users.</returns>
        Task<IEnumerable<User>> GetUsers();

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="model">CreateUserViewModel object.</param>
        /// <returns>IdentityResult.</returns>
        Task<IdentityResult> CreateUser(CreateUserViewModel model);

        /// <summary>
        /// Get change password view model.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>ChangePasswordViewModel.</returns>
        Task<ChangePasswordViewModel> GetChangePasswordViewModel(string id);

        /// <summary>
        /// Change password for user.
        /// </summary>
        /// <param name="model">ChangePasswordViewModel.</param>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>IdentityResult.</returns>
        Task<IdentityResult> ChangePassword(ChangePasswordViewModel model, HttpContext httpContext);

        /// <summary>
        /// Get edit user view model.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>EditUserViewModel object.</returns>
        Task<EditUserViewModel> GetEditUserViewModel(string id);

        /// <summary>
        /// Update user in edit method.
        /// </summary>
        /// <param name="model">EditUserViewModel object.</param>
        /// <returns>IdentityResult object.</returns>
        Task<IdentityResult> UpdateUserInEditAsync(EditUserViewModel model);

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">Id of deleting user.</param>
        /// <returns>Task with id of deleted user.</returns>
        Task<string> DeleteUserAsync(string id);

        /// <summary>
        /// Get change role view model.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>ChangeRoleViewModel object.</returns>
        Task<ChangeRoleViewModel> GetChangeRoleViewModel(string id);

        /// <summary>
        /// Edit roles for user.
        /// </summary>
        /// <param name="model">ChangeRoleViewModel.</param>
        /// <returns>Task.</returns>
        Task EditRoles(ChangeRoleViewModel model);
    }
}
