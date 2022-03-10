using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketManagement.Web.Interfaces;
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
        /// RoleManager object.
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// UserManager object.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersWebService"/> class.
        /// </summary>
        /// <param name="roleManager">RoleManager object.</param>
        /// <param name="userManager">UserManager object.</param>
        public UsersWebService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> UpdateUserInEditAsync(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.Email = model.Email;
            user.UserName = model.Email;
            user.FirstName = model.FirstName;
            user.Surname = model.Surname;
            user.Balance = model.Balance;
            user.TimeZone = model.TimeZone;

            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<string> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains("admin"))
            {
                throw new InvalidOperationException("You can't delete admin account.");
            }

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return id;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model, User user, HttpContext httpContext)
        {
            var passwordValidator = httpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
            var passwordHasher = httpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
            var result = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
            if (result.Succeeded)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                await _userManager.UpdateAsync(user);
            }

            return result;
        }

        public async Task<ChangeRoleViewModel> GetChangeRoleViewModel(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            ChangeRoleViewModel model = new ()
            {
                UserId = user.Id,
                UserEmail = user.UserName,
                UserRoles = userRoles,
                AllRoles = allRoles,
            };
            return model;
        }

        public async Task EditRolesAsync(List<string> roles, User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
        }
    }
}
