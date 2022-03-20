using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Web.Extensions;
using TicketManagement.Web.Infrastructure;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Interfaces.HttpClients;
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
        /// AccountWebService object.
        /// </summary>
        private readonly IAccountWebService _service;

        /// <summary>
        /// IPurchaseClient object.
        /// </summary>
        private readonly IPurchaseClient _purchaseClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="service">AccountWebService object.</param>
        /// <param name="purchaseClient">IPurchaseClient object.</param>
        public AccountController(IAccountWebService service,
            IPurchaseClient purchaseClient)
        {
            _service = service;
            _purchaseClient = purchaseClient;
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

            await _service.RegisterUser(model, HttpContext);
            return RedirectToAction(nameof(Index));
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

            await _service.LoginUser(model, HttpContext);
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Index), typeof(EventsController).GetControllerName());
            }
        }

        /// <summary>
        /// Logout for user.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        public async Task<IActionResult> Logout()
        {
            await _service.Logout(HttpContext);
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
            var accountViewModel = await _service.GetEditAccountViewModelForEdit(HttpContext, id);
            if (accountViewModel == null)
            {
                return NotFound();
            }

            return View(accountViewModel);
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

            var result = await _service.UpdateUserInEdit(HttpContext, model);
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
        /// Add balance on account.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet]
        public async Task<IActionResult> AddBalance(string id)
        {
            return View(await _service.GetAddBalanceViewModel(HttpContext, id));
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

            var result = await _service.AddBalanceToUser(HttpContext, model);
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
        /// Personal account.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accountVm = await _purchaseClient.GetAccountViewModelForPersonalAccount(HttpContext.GetJwtToken());
            var user = await _service.GetUserFromJwt(accountVm.JwtToken);
            var accountVmForPersonalAccout = new AccountViewModelForPersonalAccount
            {
                User = user,
                Tickets = accountVm.Tickets,
            };
            return View(accountVmForPersonalAccout);
        }
    }
}
