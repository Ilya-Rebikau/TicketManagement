using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TicketManagement.UserAPI.Interfaces;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Models.Account;

namespace TicketManagement.UserAPI.Services
{
    /// <summary>
    /// Service for account controller.
    /// </summary>
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Base role for new registering user.
        /// </summary>
        private readonly string _baseRoleForNewUser;

        /// <summary>
        /// UserManager object.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// SignInManager object.
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// IConverter object.
        /// </summary>
        private readonly IConverter<User, EditAccountModel> _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="signInManager">SignInManager object.</param>
        /// <param name="configuration">IConfiguration object.</param>
        /// <param name="converter">IConverter object.</param>
        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IConverter<User, EditAccountModel> converter)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _baseRoleForNewUser = configuration.GetValue<string>("BaseRole");
            _converter = converter;
        }

        public async Task<RegisterResultModel> RegisterUser(RegisterModel model)
        {
            var user = new User { Email = model.Email, UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, _baseRoleForNewUser);
            IList<string> roles = new List<string>();
            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
                roles = await _userManager.GetRolesAsync(user);
                await _signInManager.SignInAsync(user, false);
            }

            var registerResult = new RegisterResultModel
            {
                User = user,
                Roles = roles,
                IdentityResult = result,
            };
            return registerResult;
        }

        public async Task<LoginResultModel> Login(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            var loginResult = new LoginResultModel
            {
                SignInResult = result,
            };
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                loginResult.User = user;
                loginResult.Roles = await _userManager.GetRolesAsync(user);
            }

            return loginResult;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<EditAccountModel> GetEditAccountViewModelForEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return await _converter.ConvertSourceToDestination(user);
        }

        public async Task<IdentityResult> UpdateUserInEdit(EditAccountModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.Email = model.Email;
            user.UserName = model.Email;
            user.FirstName = model.FirstName;
            user.Surname = model.Surname;
            user.TimeZone = model.TimeZone;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<AddBalanceModel> GetBalanceViewModel(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var model = new AddBalanceModel
            {
                Id = user.Id,
                Balance = user.Balance,
            };
            return model;
        }

        public async Task<IdentityResult> AddBalance(AddBalanceModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.Balance += model.Balance;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> GetUserId(string token)
        {
            var user = await _userManager.FindByEmailAsync(GetUserEmail(token));
            return user.Id;
        }

        public async Task<bool> ChangeBalanceForUser(string token, double price)
        {
            var user = await _userManager.FindByEmailAsync(GetUserEmail(token));
            if (user.Balance >= price)
            {
                user.Balance -= price;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get user email from jwt token.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <returns>Email of user.</returns>
        private static string GetUserEmail(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims;
            var identity = new ClaimsIdentity(claims);
            var principalClaims = new ClaimsPrincipal(identity);
            var email = principalClaims.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            return email;
        }
    }
}
