using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketManagement.UserAPI.Interfaces;
using TicketManagement.UserAPI.Models;
using TicketManagement.UserAPI.Models.Users;

namespace TicketManagement.UserAPI.Services
{
    /// <summary>
    /// Web service for users controller.
    /// </summary>
    public class UsersService : IUsersService
    {
        /// <summary>
        /// Base role for creating user.
        /// </summary>
        private const string BaseUserRole = "user";

        /// <summary>
        /// UserManager object.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// RoleManager object.
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="roleManager">RoleManager object.</param>
        public UsersService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
            if (user is null)
            {
                return null;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains("admin"))
            {
                throw new InvalidOperationException("You can't delete admin account.");
            }

            await _userManager.DeleteAsync(user);
            return id;
        }

        public async Task<ChangeRoleViewModel> GetChangeRoleViewModel(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            var model = new ChangeRoleViewModel
            {
                UserId = user.Id,
                UserEmail = user.UserName,
                UserRoles = userRoles,
                AllRoles = allRoles,
            };
            return model;
        }

        public async Task EditRoles(ChangeRoleViewModel model)
        {
            model.AllRoles = _roleManager.Roles.ToList();
            var user = await _userManager.FindByIdAsync(model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = model.UserRoles.Except(userRoles).ToList();
            var removedRoles = userRoles.Except(model.UserRoles).ToList();
            await _userManager.AddToRolesAsync(user, addedRoles);
            await _userManager.RemoveFromRolesAsync(user, removedRoles);
        }

        public Task<IEnumerable<User>> GetUsers()
        {
            return Task.FromResult(_userManager.Users.AsEnumerable());
        }

        public async Task<IdentityResult> CreateUser(CreateUserViewModel model)
        {
            var user = new User { Email = model.Email, UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, BaseUserRole);
            }

            return result;
        }

        public async Task<EditUserViewModel> GetEditUserViewModel(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Balance = user.Balance,
                TimeZone = user.TimeZone,
            };
            return model;
        }

        public async Task<ChangePasswordViewModel> GetChangePasswordViewModel(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var model = new ChangePasswordViewModel
            {
                Id = user.Id,
                Email = user.Email,
            };
            return model;
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordViewModel model, HttpContext httpContext)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null)
            {
                throw new InvalidOperationException("User not found");
            }

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
    }
}
