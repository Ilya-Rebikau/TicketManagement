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
    public interface IUsersClient
    {
        [Post("account/register")]
        public Task<string> Register([Body] RegisterViewModel userModel, CancellationToken cancellationToken = default);

        [Post("account/login")]
        public Task<string> Login([Body] LoginViewModel userModel, CancellationToken cancellationToken = default);

        [Post("account/logout")]
        public Task Logout([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("account/edit/{id}")]
        public Task<EditAccountViewModel> Edit([Header("Authorization")] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Post("account/edit")]
        public Task<IdentityResult> Edit([Header("Authorization")] string token, [Body] EditAccountViewModel model, CancellationToken cancellationToken = default);

        [Get("account/addbalance/{id}")]
        public Task<AddBalanceViewModel> GetAddBalanceViewModel([Header("Authorization")] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Post("account/addbalance")]
        public Task<IdentityResult> AddBalance([Header("Authorization")] string token, [Body] AddBalanceViewModel model, CancellationToken cancellationToken = default);

        [Post("account/getuserid")]
        public Task<string> GetUserId([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Get("users/getusers")]
        public Task<IEnumerable<User>> GetUsers([Header("Authorization")] string token, CancellationToken cancellationToken = default);

        [Post("users/create")]
        public Task<IdentityResult> CreateUser([Header("Authorization")] string token, [Body] CreateUserViewModel model, CancellationToken cancellationToken = default);

        [Get("users/edit/{id}")]
        public Task<EditUserViewModel> GetEditUserViewModel([Header("Authorization")] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Post("users/edit")]
        public Task<IdentityResult> EditUser([Header("Authorization")] string token, [Body] EditUserViewModel model, CancellationToken cancellationToken = default);

        [Post("users/delete/{id}")]
        public Task<string> DeleteUser([Header("Authorization")] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Get("users/changepassword/{id}")]
        public Task<ChangePasswordViewModel> GetChangePasswordViewModel([Header("Authorization")] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Post("users/changepassword")]
        public Task<IdentityResult> ChangePassword([Header("Authorization")] string token, [Body] ChangePasswordViewModel model, CancellationToken cancellationToken = default);

        [Get("users/editroles/{id}")]
        public Task<ChangeRoleViewModel> GetChangeRoleViewModel([Header("Authorization")] string token, [Path] string id, CancellationToken cancellationToken = default);

        [Post("users/editroles")]
        public Task ChangeRoles([Header("Authorization")] string token, [Body] ChangeRoleViewModel model, CancellationToken cancellationToken = default);
    }
}
