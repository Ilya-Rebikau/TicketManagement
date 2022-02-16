using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// Initializes a new instance of the <see cref="AccountWebService"/> class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="signInManager">SignInManager object.</param>
        /// <param name="ticketService">TicketService object.</param>
        /// <param name="eventService">EventService object.</param>
        /// <param name="eventAreaService">EventAreaService object.</param>
        /// <param name="eventSeatService">EventSeatService object.</param>
        /// <param name="converter">ConverterForTime object.</param>
        public AccountWebService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IService<TicketDto> ticketService,
            IService<EventDto> eventService,
            IService<EventAreaDto> eventAreaService,
            IService<EventSeatDto> eventSeatService,
            ConverterForTime converter)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ticketService = ticketService;
            _eventService = eventService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _converter = converter;
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
    }
}
