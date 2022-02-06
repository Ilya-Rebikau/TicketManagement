using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.Web.Models;
using TicketManagement.Web.Models.Events;

namespace TicketManagement.Web.Controllers
{
    [ResponseCache(CacheProfileName = "Caching")]
    public class EventsController : Controller
    {
        private readonly IService<EventDto> _service;
        private readonly IService<TicketDto> _ticketService;
        private readonly IService<EventAreaDto> _eventAreaService;
        private readonly IService<EventSeatDto> _eventSeatService;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<EventsController> _localizer;

        public EventsController(IService<EventDto> service, IService<TicketDto> ticketService, IService<EventAreaDto> eventAreaService,
            IService<EventSeatDto> eventSeatService, UserManager<User> userManager, IStringLocalizer<EventsController> localizer)
        {
            _service = service;
            _ticketService = ticketService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _userManager = userManager;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _service.GetByIdAsync((int)id);
            if (@event == null)
            {
                return NotFound();
            }

            var eventAreas = await _eventAreaService.GetAllAsync();
            var eventAreasForEvent = eventAreas.Where(x => x.EventId == (int)id);
            var eventSeats = await _eventSeatService.GetAllAsync();
            var eventAreaViewModels = new List<EventAreaViewModel>();
            foreach (var eventArea in eventAreasForEvent)
            {
                var eventSeatsInArea = eventSeats.Where(x => x.EventAreaId == eventArea.Id).ToList();
                var eventAreaViewModel = new EventAreaViewModel
                {
                    EventArea = eventArea,
                    EventSeats = eventSeatsInArea,
                };

                eventAreaViewModels.Add(eventAreaViewModel);
            }

            var eventViewModel = new EventViewModel
            {
                EventAreas = eventAreaViewModels,
                Event = @event,
            };

            return View(eventViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventDto @event)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(@event);
                return RedirectToAction(nameof(Index));
            }

            return View(@event);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingEvent = await _service.GetByIdAsync((int)id);
            if (updatingEvent == null)
            {
                return NotFound();
            }

            return View(updatingEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventDto @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(@event);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(@event);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingEvent = await _service.GetByIdAsync((int)id);
            if (deletingEvent == null)
            {
                return NotFound();
            }

            return View(deletingEvent);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(@event);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int? eventSeatId, double? price)
        {
            if (eventSeatId == null || price == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var ticket = new TicketDto
            {
                UserId = user.Id,
                EventSeatId = (int)eventSeatId,
            };

            var ticketVm = new TicketViewModel
            {
                Ticket = ticket,
                Price = (double)price,
            };

            return View(ticketVm);
        }

        [HttpPost]
        [ActionName("Buy")]
        public async Task<IActionResult> BuyConfirmed(TicketViewModel ticketVm)
        {
            var user = await _userManager.FindByIdAsync(ticketVm.Ticket.UserId);
            if (user.Balance >= ticketVm.Price)
            {
                user.Balance -= ticketVm.Price;
                var seat = await _eventSeatService.GetByIdAsync(ticketVm.Ticket.EventSeatId);
                seat.State = PlaceStatus.Occupied;
                await _eventSeatService.UpdateAsync(seat);
                await _ticketService.CreateAsync(ticketVm.Ticket);
            }
            else
            {
                return ValidationProblem($"{_localizer["NoBalance"]}");
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EventExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
