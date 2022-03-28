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
        /// Get all users.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>All users.</returns>
        Task<IEnumerable<User>> GetUsers(HttpContext httpContext, int pageNumber);

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="model">CreateUserViewModel object.</param>
        /// <returns>IdentityResult.</returns>
        Task<IdentityResult> CreateUser(HttpContext httpContext, CreateUserViewModel model);

        /// <summary>
        /// Get EditUserViewModel object.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="id">Id of user.</param>
        /// <returns>EditUserViewModel.</returns>
        Task<EditUserViewModel> GetEditUserViewModel(HttpContext httpContext, string id);

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="model">EditUserViewModel object.</param>
        /// <returns>EditUserViewModel.</returns>
        Task<IdentityResult> EditUser(HttpContext httpContext, EditUserViewModel model);

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="id">Id of deleting user.</param>
        /// <returns>Id of deleted user.</returns>
        Task<string> DeleteUser(HttpContext httpContext, string id);

        /// <summary>
        /// Get ChangePasswordViewModel object.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="id">User id.</param>
        /// <returns>ChangePasswordViewModel object.</returns>
        Task<ChangePasswordViewModel> GetChangePasswordViewModel(HttpContext httpContext, string id);

        /// <summary>
        /// Change password for user.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="model">ChangePasswordViewModel object.</param>
        /// <returns>IdentityResult object.</returns>
        Task<IdentityResult> ChangePassowrd(HttpContext httpContext, ChangePasswordViewModel model);

        /// <summary>
        /// Change roles for user.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="id">User id.</param>
        /// <returns>ChangeRoleViewModel object.</returns>
        Task<ChangeRoleViewModel> GetChangeRoleViewModel(HttpContext httpContext, string id);

        /// <summary>
        /// Change roles for user.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <param name="model">ChangeRoleViewModel object.</param>
        /// <returns>Task.</returns>
        Task ChangeRoles(HttpContext httpContext, ChangeRoleViewModel model);
    }
}
