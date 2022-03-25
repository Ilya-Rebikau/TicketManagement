using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RestEase;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Account;
using TicketManagement.Web.Models.Users;

namespace TicketManagement.Web.Interfaces.HttpClients
{
    /// <summary>
    /// Client for UsersAPI.
    /// </summary>
    public interface IUsersClient
    {
        /// <summary>
        /// Key with authorization string in header.
        /// </summary>
        private const string AuthorizationKey = "Authorization";

        [Post("account/register")]
        public Task<string> Register([Body] RegisterViewModel userModel, CancellationToken cancellationToken = default);

        [Post("account/login")]
        public Task<string> Login([Body] LoginViewModel userModel, CancellationToken cancellationToken = default);

        [Post("account/logout")]
        public Task Logout([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("account/edit/{id}")]
        public Task<EditAccountViewModel> Edit([Header(AuthorizationKey)] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Put("account/edit")]
        public Task<IdentityResult> Edit([Header(AuthorizationKey)] string token, [Body] EditAccountViewModel model, CancellationToken cancellationToken = default);

        [Get("account/addbalance/{id}")]
        public Task<AddBalanceViewModel> GetAddBalanceViewModel([Header(AuthorizationKey)] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Put("account/addbalance")]
        public Task<IdentityResult> AddBalance([Header(AuthorizationKey)] string token, [Body] AddBalanceViewModel model, CancellationToken cancellationToken = default);

        [Post("account/getuserid")]
        public Task<string> GetUserId([Header(AuthorizationKey)] string token, CancellationToken cancellationToken = default);

        [Get("users/getusers")]
        public Task<IEnumerable<User>> GetUsers([Header(AuthorizationKey)] string token, [Body] int pageNumber, CancellationToken cancellationToken = default);

        [Post("users/create")]
        public Task<IdentityResult> CreateUser([Header(AuthorizationKey)] string token, [Body] CreateUserViewModel model, CancellationToken cancellationToken = default);

        [Get("users/edit/{id}")]
        public Task<EditUserViewModel> GetEditUserViewModel([Header(AuthorizationKey)] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Put("users/edit")]
        public Task<IdentityResult> EditUser([Header(AuthorizationKey)] string token, [Body] EditUserViewModel model, CancellationToken cancellationToken = default);

        [Delete("users/delete/{id}")]
        public Task<string> DeleteUser([Header(AuthorizationKey)] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Get("users/changepassword/{id}")]
        public Task<ChangePasswordViewModel> GetChangePasswordViewModel([Header(AuthorizationKey)] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Put("users/changepassword")]
        public Task<IdentityResult> ChangePassword([Header(AuthorizationKey)] string token, [Body] ChangePasswordViewModel model, CancellationToken cancellationToken = default);

        [Get("users/editroles/{id}")]
        public Task<ChangeRoleViewModel> GetChangeRoleViewModel([Header(AuthorizationKey)] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Put("users/editroles")]
        public Task ChangeRoles([Header(AuthorizationKey)] string token, [Body] ChangeRoleViewModel model, CancellationToken cancellationToken = default);
    }
}
