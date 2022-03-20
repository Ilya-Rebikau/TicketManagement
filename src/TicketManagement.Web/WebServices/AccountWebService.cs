using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Interfaces.HttpClients;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Account;

namespace TicketManagement.Web.WebServices
{
    /// <summary>
    /// Web service for account controller.
    /// </summary>
    public class AccountWebService : IAccountWebService
    {
        /// <summary>
        /// UserManager object.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// SignInManager object.
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// IUserClient object.
        /// </summary>
        private readonly IUsersClient _userClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountWebService"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="signInManager">SignInManager object.</param>
        /// <param name="userClient">IUserClient object.</param>
        public AccountWebService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUsersClient userClient)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userClient = userClient;
        }

        public async Task RegisterUser(RegisterViewModel model, HttpContext httpContext)
        {
            var token = await _userClient.Register(model);
            await SignIn(token, httpContext);
        }

        public async Task LoginUser(LoginViewModel model, HttpContext httpContext)
        {
            var token = await _userClient.Login(model);
            await SignIn(token, httpContext);
        }

        public async Task Logout(HttpContext httpContext)
        {
            await _userClient.Logout(httpContext.GetJwtToken());
            await _signInManager.SignOutAsync();
        }

        public async Task<EditAccountViewModel> GetEditAccountViewModelForEdit(HttpContext httpContext, string id)
        {
            return await _userClient.Edit(httpContext.GetJwtToken(), id);
        }

        public async Task<IdentityResult> UpdateUserInEdit(HttpContext httpContext, EditAccountViewModel model)
        {
            return await _userClient.Edit(httpContext.GetJwtToken(), model);
        }

        public async Task<AddBalanceViewModel> GetAddBalanceViewModel(HttpContext httpContext, string id)
        {
            return await _userClient.GetAddBalanceViewModel(httpContext.GetJwtToken(), id);
        }

        public async Task<IdentityResult> AddBalanceToUser(HttpContext httpContext, AddBalanceViewModel model)
        {
            return await _userClient.AddBalance(httpContext.GetJwtToken(), model);
        }

        public async Task<User> GetUserFromJwt(string token)
        {
            var userId = await _userClient.GetUserId(token);
            return await _userManager.FindByIdAsync(userId);
        }

        /// <summary>
        /// Sign in app.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>Task.</returns>
        private async Task SignIn(string token, HttpContext httpContext)
        {
            if (string.IsNullOrEmpty(token) || httpContext is null)
            {
                throw new InvalidOperationException("Wrong jwt token or http context");
            }

            httpContext.AppendCookies(token);
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var user = await _userManager.FindByEmailAsync(jwtSecurityToken.Subject);
            await _signInManager.SignInAsync(user, false);
        }
    }
}
