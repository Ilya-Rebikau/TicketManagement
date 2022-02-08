using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Roles;
using TicketManagement.Web.Models.Users;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for users.
    /// </summary>
    [Authorize(Roles = "admin")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class UsersController : Controller
    {
        /// <summary>
        /// UserManager object.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// RoleManager object.
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Localizer object.
        /// </summary>
        private readonly IStringLocalizer<UsersController> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="roleManager">RoleManager object.</param>
        /// <param name="localizer">Localizer object.</param>
        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IStringLocalizer<UsersController> localizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _localizer = localizer;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Index() => View(_userManager.Users.ToList());

        /// <summary>
        /// Create user.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Create() => View();

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="model">CreateUserViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="id">Id of editing user.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new () { Id = user.Id, Email = user.Email, FirstName = user.FirstName, Surname = user.Surname, Balance = user.Balance, TimeZone = user.TimeZone };
            return View(model);
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="model">EditUserViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.FirstName = model.FirstName;
                    user.Surname = model.Surname;
                    user.Balance = model.Balance;
                    user.TimeZone = model.TimeZone;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">Id of deleting object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Change passowrd for user.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]

        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ChangePasswordViewModel model = new () { Id = user.Id, Email = user.Email };
            return View(model);
        }

        /// <summary>
        /// Change password for user.
        /// </summary>
        /// <param name="model">ChangePasswordViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    var result = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"{_localizer["UserNotFound"]}");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Edit roles.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> EditRoles(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
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
                return View(model);
            }

            return NotFound();
        }

        /// <summary>
        /// Edit roles.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="roles">All roles.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> EditRoles(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
