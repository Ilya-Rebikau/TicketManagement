using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Account;

namespace TicketManagement.Web.Controllers
{
    [ResponseCache(CacheProfileName = "Caching")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IService<TicketDto> _ticketService;
        private readonly IService<EventDto> _eventService;
        private readonly IService<EventAreaDto> _eventAreaService;
        private readonly IService<EventSeatDto> _eventSeatService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IService<TicketDto> ticketService,
            IService<EventDto> eventService, IService<EventAreaDto> eventAreaService, IService<EventSeatDto> eventSeatService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ticketService = ticketService;
            _eventService = eventService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

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

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

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
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Events");
        }

        [Authorize(Roles = "admin, user, event manager, venue manager")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            EditAccountViewModel model = new () { Id = user.Id, Email = user.Email, FirstName = user.FirstName, Surname = user.Surname };
            return View(model);
        }

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
                var ticketVm = new AccountTicketViewModel
                {
                    Price = eventArea.Price,
                    Event = await _eventService.GetByIdAsync(eventArea.EventId),
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
    }
}
