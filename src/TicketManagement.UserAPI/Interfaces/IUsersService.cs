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
        /// <param name="pageNumber">Page number.</param>
        /// <returns>All users.</returns>
        Task<IEnumerable<User>> GetUsers(int pageNumber);

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="model">CreateUserViewModel object.</param>
        /// <returns>IdentityResult.</returns>
        Task<IdentityResult> CreateUser(CreateUserModel model);

        /// <summary>
        /// Get change password view model.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>ChangePasswordViewModel.</returns>
        Task<ChangePasswordModel> GetChangePasswordViewModel(string id);

        /// <summary>
        /// Change password for user.
        /// </summary>
        /// <param name="model">ChangePasswordViewModel.</param>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>IdentityResult.</returns>
        Task<IdentityResult> ChangePassword(ChangePasswordModel model, HttpContext httpContext);

        /// <summary>
        /// Get edit user view model.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>EditUserViewModel object.</returns>
        Task<EditUserModel> GetEditUserViewModel(string id);

        /// <summary>
        /// Update user in edit method.
        /// </summary>
        /// <param name="model">EditUserViewModel object.</param>
        /// <returns>IdentityResult object.</returns>
        Task<IdentityResult> UpdateUserInEditAsync(EditUserModel model);

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
        Task<ChangeRoleModel> GetChangeRoleViewModel(string id);

        /// <summary>
        /// Edit roles for user.
        /// </summary>
        /// <param name="model">ChangeRoleViewModel.</param>
        /// <returns>Task.</returns>
        Task EditRoles(ChangeRoleModel model);
    }
}
