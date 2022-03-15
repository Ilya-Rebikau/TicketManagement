using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Interfaces;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Account;

namespace TicketManagement.Web.WebServices
{
    /// <summary>
    /// Web service for account controller.
    /// </summary>
    public class AccountWebService : IAccountWebService
    {
        /// <summary>
        /// Cookies name.
        /// </summary>
        private const string CookiesKey = "Cookies";

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
        /// Converter for time object.
        /// </summary>
        private readonly ConverterForTime _converter;

        /// <summary>
        /// IUserClient object.
        /// </summary>
        private readonly IUserClient _userClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountWebService"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="signInManager">SignInManager object.</param>
        /// <param name="ticketService">TicketService object.</param>
        /// <param name="eventService">EventService object.</param>
        /// <param name="eventAreaService">EventAreaService object.</param>
        /// <param name="eventSeatService">EventSeatService object.</param>
        /// <param name="converter">ConverterForTime object.</param>
        /// <param name="userClient">IUserClient object.</param>
#pragma warning disable S107 // Methods should not have too many parameters
        public AccountWebService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IService<TicketDto> ticketService,
            IService<EventDto> eventService,
            IService<EventAreaDto> eventAreaService,
            IService<EventSeatDto> eventSeatService,
            ConverterForTime converter,
            IUserClient userClient)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ticketService = ticketService;
            _eventService = eventService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _converter = converter;
            _userClient = userClient;
        }

        public async Task RegisterUser(RegisterViewModel model, HttpContext httpContext)
        {
            var token = await _userClient.Register(model);
            await SignIn(token, httpContext);
        }

        public async Task LoginUser(LoginViewModel model, HttpContext httpContext)
        {
            var token = await _userClient.Login(model);
            await SignIn(token, httpContext);
        }

        public async Task Logout(HttpContext httpContext)
        {
            string token = GetJwtToken(httpContext);
            await _userClient.Logout(token);
            await _signInManager.SignOutAsync();
        }

        public async Task<EditAccountViewModel> GetEditAccountViewModelForEdit(HttpContext httpContext, string id)
        {
            return await _userClient.Edit(GetJwtToken(httpContext), id);
        }

        public async Task<IdentityResult> UpdateUserInEdit(HttpContext httpContext, EditAccountViewModel model)
        {
            return await _userClient.Edit(GetJwtToken(httpContext), model);
        }

        public async Task<IdentityResult> AddBalanceToUser(AddBalanceViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.Balance += model.Balance;
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<AccountViewModel> GetAccountViewModelInIndex(User user)
        {
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
                _converter.ConvertTimeForUser(@event, user);
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
            return accountVm;
        }

        /// <summary>
        /// Get jwt token from http context.
        /// </summary>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>Jwt token.</returns>
        private static string GetJwtToken(HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue(CookiesKey, out var token);
            return token;
        }

        /// <summary>
        /// Sign in app.
        /// </summary>
        /// <param name="token">Jwt token.</param>
        /// <param name="httpContext">HttpContext object.</param>
        /// <returns>Task.</returns>
        private async Task SignIn(string token, HttpContext httpContext)
        {
            if (string.IsNullOrEmpty(token) || httpContext is null)
            {
                throw new InvalidOperationException("Wrong jwt token or http context");
            }

            httpContext.Response.Cookies.Append(CookiesKey, token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
            });
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var user = await _userManager.FindByEmailAsync(jwtSecurityToken.Subject);
            await _signInManager.SignInAsync(user, false);
        }
    }
}
