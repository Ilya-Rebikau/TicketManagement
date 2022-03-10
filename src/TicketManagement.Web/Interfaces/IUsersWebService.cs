using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Users;

namespace TicketManagement.Web.Interfaces
{
    /// <summary>
    /// Web service for users controller.
    /// </summary>
    public interface IUsersWebService
    {
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
        /// Change password for user.
        /// </summary>
        /// <param name="model">ChangePasswordViewModel object.</param>
        /// <param name="user">User.</param>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>IdentityResult object.</returns>
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model, User user, HttpContext httpContext);

        /// <summary>
        /// Get change role view model.
        /// </summary>
        /// <param name="user">User.</param>
        /// <returns>ChangeRoleViewModel object.</returns>
        Task<ChangeRoleViewModel> GetChangeRoleViewModel(User user);

        /// <summary>
        /// Edit roles for user.
        /// </summary>
        /// <param name="roles">List of all roles.</param>
        /// <param name="user">User.</param>
        /// <returns>Task.</returns>
        Task EditRolesAsync(List<string> roles, User user);
    }
}
