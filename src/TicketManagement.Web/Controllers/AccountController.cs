using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Account;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for user account.
    /// </summary>
    [ResponseCache(CacheProfileName = "Caching")]
    [ExceptionFilter]
    public class AccountController : Controller
    {
        /// <summary>
        /// Const for showing error with wrong login or/and password from resource file.
        /// </summary>
        private const string WrongLoginPasswordResxKey = "WrongLoginPassword";

        /// <summary>
        /// AccountWebService object.
        /// </summary>
        private readonly IAccountWebService _service;

        /// <summary>
        /// UserManager object.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// SignInManager object.
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        /// <summary>
        /// Localizer object.
        /// </summary>
        private readonly IStringLocalizer<AccountController> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="service">AccountWebService object.</param>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="signInManager">SignInManager object.</param>
        /// <param name="localizer">Localizer object.</param>
        public AccountController(IAccountWebService service,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IStringLocalizer<AccountController> localizer)
        {
            _service = service;
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="model">RegisterViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.RegisterUser(model);
            if (result.Succeeded)
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
        /// Login for user.
        /// </summary>
        /// <param name="returnUrl">Url to back.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Login for user.
        /// </summary>
        /// <param name="model">LoginViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index), typeof(EventsController).GetControllerName());
                }
            }
            else
            {
                ModelState.AddModelError("", _localizer[WrongLoginPasswordResxKey]);
            }

            return View(model);
        }

        /// <summary>
        /// Logout for user.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), typeof(EventsController).GetControllerName());
        }

        /// <summary>
        /// Edit account.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditAccountViewModel model = user;
            return View(model);
        }

        /// <summary>
        /// Edit user.
        /// </summary>
        /// <param name="model">EditAccountViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                var result = await _service.UpdateUserInEdit(model, user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Add balance on account.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet]
        public async Task<IActionResult> AddBalance(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            AddBalanceViewModel model = new () { Id = user.Id, Balance = user.Balance };
            return View(model);
        }

        /// <summary>
        /// Add balance on account.
        /// </summary>
        /// <param name="model">AddBalanceViewModel object.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost]
        public async Task<IActionResult> AddBalance(AddBalanceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.AddBalanceToUser(model);
            if (result.Succeeded)
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
        /// Personal account.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound();
            }

            return View(await _service.GetAccountViewModelInIndex(user));
        }
    }
}
