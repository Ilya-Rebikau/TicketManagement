using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketManagement.UserAPI.Interfaces;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Models.Account;
using UserAPI.Models.Account;

namespace TicketManagement.UserAPI.Services
{
    /// <summary>
    /// Service for account controller.
    /// </summary>
    public class AccountService : IAccountService
    {
        private const string UserRole = "user";

        /// <summary>
        /// UserManager object.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// SignInManager object.
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="signInManager">SignInManager object.</param>
        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<RegisterResult> RegisterUser(RegisterViewModel model)
        {
            var user = new User { Email = model.Email, UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, UserRole);
            IList<string> roles = new List<string>();
            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
                roles = await _userManager.GetRolesAsync(user);
                await _signInManager.SignInAsync(user, false);
            }

            var registerResult = new RegisterResult
            {
                User = user,
                Roles = roles,
                IdentityResult = result,
            };
            return registerResult;
        }

        public async Task<LoginResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            var loginResult = new LoginResult
            {
                SignInResult = result,
            };
            if (result.Succeeded)
            {
                loginResult.User = await _userManager.FindByNameAsync(model.Email);
            }

            return loginResult;
        }

        public async Task<IdentityResult> UpdateUserInEdit(EditAccountViewModel model, User user)
        {
            user.Email = model.Email;
            user.UserName = model.Email;
            user.FirstName = model.FirstName;
            user.Surname = model.Surname;
            user.TimeZone = model.TimeZone;

            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> AddBalanceToUser(AddBalanceViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.Balance += model.Balance;
            var result = await _userManager.UpdateAsync(user);
            return result;
        }
    }
}
