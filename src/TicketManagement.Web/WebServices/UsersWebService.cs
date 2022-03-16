using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Users;

namespace TicketManagement.Web.WebServices
{
    /// <summary>
    /// Web service for users controller.
    /// </summary>
    public class UsersWebService : IUsersWebService
    {
        /// <summary>
        /// IUsersClient object.
        /// </summary>
        private readonly IUsersClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersWebService"/> class.
        /// </summary>
        /// <param name="client">IUsersClient object.</param>
        public UsersWebService(IUsersClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<User>> GetUsers(HttpContext httpContext)
        {
            return await _client.GetUsers(httpContext.GetJwtToken());
        }

        public async Task<IdentityResult> CreateUser(HttpContext httpContext, CreateUserViewModel model)
        {
            return await _client.CreateUser(httpContext.GetJwtToken(), model);
        }

        public async Task<EditUserViewModel> GetEditUserViewModel(HttpContext httpContext, string id)
        {
            return await _client.GetEditUserViewModel(httpContext.GetJwtToken(), id);
        }

        public async Task<IdentityResult> EditUser(HttpContext httpContext, EditUserViewModel model)
        {
            return await _client.EditUser(httpContext.GetJwtToken(), model);
        }

        public async Task<string> DeleteUser(HttpContext httpContext, string id)
        {
            return await _client.DeleteUser(httpContext.GetJwtToken(), id);
        }

        public async Task<ChangePasswordViewModel> GetChangePasswordViewModel(HttpContext httpContext, string id)
        {
            return await _client.GetChangePasswordViewModel(httpContext.GetJwtToken(), id);
        }

        public async Task<IdentityResult> ChangePassowrd(HttpContext httpContext, ChangePasswordViewModel model)
        {
            return await _client.ChangePassword(httpContext.GetJwtToken(), model);
        }

        public async Task<ChangeRoleViewModel> GetChangeRoleViewModel(HttpContext httpContext, string id)
        {
            return await _client.GetChangeRoleViewModel(httpContext.GetJwtToken(), id);
        }

        public async Task ChangeRoles(HttpContext httpContext, ChangeRoleViewModel model)
        {
            await _client.ChangeRoles(httpContext.GetJwtToken(), model);
        }
    }
}
