using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketManagement.UserAPI.Interfaces;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Models.Account;

namespace TicketManagement.UserAPI.Services
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
        /// Initializes a new instance of the <see cref="AccountWebService"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="signInManager">SignInManager object.</param>
        public AccountWebService(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUser(RegisterViewModel model)
        {
            var user = new User { Email = model.Email, UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "user");
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }

            return result;
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
