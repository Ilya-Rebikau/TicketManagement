using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Users;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for users.
    /// </summary>
    [Authorize(Roles = "admin")]
    [ExceptionFilter]
    public class UsersController : Controller
    {
        /// <summary>
        /// UsersWebService object.
        /// </summary>
        private readonly IUsersWebService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="service">UsersWebService object.</param>
        public UsersController(IUsersWebService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var users = await _service.GetUsers(HttpContext, pageNumber);
            var nextUsers = await _service.GetUsers(HttpContext, pageNumber + 1);
            PageViewModel.NextPage = nextUsers is not null && nextUsers.Any();
            PageViewModel.PageNumber = pageNumber;
            return View(users);
        }

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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.CreateUser(HttpContext, model);
            if (!result.Errors.Any())
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
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
            return View(await _service.GetEditUserViewModel(HttpContext, id));
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="model">EditUserViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.EditUser(HttpContext, model);
            if (!result.Errors.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
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
            await _service.DeleteUser(HttpContext, id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Change passowrd for user.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpGet]

        public async Task<IActionResult> ChangePassword(string id)
        {
            return View(await _service.GetChangePasswordViewModel(HttpContext, id));
        }

        /// <summary>
        /// Change password for user.
        /// </summary>
        /// <param name="model">ChangePasswordViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.ChangePassowrd(HttpContext, model);
            if (!result.Errors.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
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
            return View(await _service.GetChangeRoleViewModel(HttpContext, userId));
        }

        /// <summary>
        /// Edit roles.
        /// </summary>
        /// <param name="model">ChangeRoleViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> EditRoles(ChangeRoleViewModel model)
        {
            await _service.ChangeRoles(HttpContext, model);
            return RedirectToAction(nameof(Index));
        }
    }
}
