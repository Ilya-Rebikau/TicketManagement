using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Account;

namespace TicketManagement.Web.Controllers
{
    /// <summary>
    /// Controller for user account.
    /// </summary>
    [ResponseCache(CacheProfileName = "Caching")]
    public class AccountController : Controller
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
        /// TicketService object.
        /// </summary>
        private readonly IService<TicketDto> _ticketService;

        /// <summary>
        /// EventService object.
        /// </summary>
        private readonly IService<EventDto> _eventService;

        /// <summary>
        /// EventAreaService object.
        /// </summary>
        private readonly IService<EventAreaDto> _eventAreaService;

        /// <summary>
        /// EventSeatService object.
        /// </summary>
        private readonly IService<EventSeatDto> _eventSeatService;

        /// <summary>
        /// Localizer object.
        /// </summary>
        private readonly IStringLocalizer<AccountController> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="signInManager">SignInManager object.</param>
        /// <param name="ticketService">TicketService object.</param>
        /// <param name="eventService">EventService object.</param>
        /// <param name="eventAreaService">EventAreaService object.</param>
        /// <param name="eventSeatService">EventSeatService object.</param>
        /// <param name="localizer">Localizer object.</param>
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IService<TicketDto> ticketService,
            IService<EventDto> eventService, IService<EventAreaDto> eventAreaService, IService<EventSeatDto> eventSeatService, IStringLocalizer<AccountController> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ticketService = ticketService;
            _eventService = eventService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
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
            if (ModelState.IsValid)
            {
                User user = new () { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
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
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Events");
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"{_localizer["WrongLoginPassword"]}");
                }
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
            return RedirectToAction("Index", "Events");
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
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditAccountViewModel model = new ()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                Surname = user.Surname,
                TimeZone = user.TimeZone,
            };
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
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.FirstName = model.FirstName;
                    user.Surname = model.Surname;
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
        /// Add balance on account.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet]
        public async Task<IActionResult> AddBalance(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
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
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Balance += model.Balance;

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
        /// Personal account.
        /// </summary>
        /// <returns>Task with IActionResult.</returns>
        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var tickets = await _ticketService.GetAllAsync();
            var usersTickets = tickets.Where(t => t.UserId == user.Id).ToList();
            var eventSeats = await _eventSeatService.GetAllAsync();
            var eventAreas = await _eventAreaService.GetAllAsync();
            var ticketsVm = new List<AccountTicketViewModel>();
            foreach (var ticket in usersTickets)
            {
                var ticketEventSeat = eventSeats.FirstOrDefault(s => s.Id == ticket.EventSeatId);
                var eventArea = eventAreas.FirstOrDefault(a => a.Id == ticketEventSeat.EventAreaId);
                var @event = await _eventService.GetByIdAsync(eventArea.EventId);
                await ConvertTimeForUser(@event);
                var ticketVm = new AccountTicketViewModel
                {
                    Price = eventArea.Price,
                    Event = @event,
                };
                ticketsVm.Add(ticketVm);
            }

            var accountVm = new AccountViewModel
            {
                User = user,
                Tickets = ticketsVm,
            };

            if (user == null)
            {
                return NotFound();
            }

            return View(accountVm);
        }

        /// <summary>
        /// Convert time from UTC to user time zone.
        /// </summary>
        /// <param name="event">Event object.</param>
        /// <returns>Task.</returns>
        private async Task ConvertTimeForUser(EventDto @event)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user is not null && !string.IsNullOrWhiteSpace(user.TimeZone))
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
                @event.TimeStart = TimeZoneInfo.ConvertTime(@event.TimeStart, TimeZoneInfo.Utc, userTimeZone);
                @event.TimeEnd = TimeZoneInfo.ConvertTime(@event.TimeEnd, TimeZoneInfo.Utc, userTimeZone);
            }
        }
    }
}
