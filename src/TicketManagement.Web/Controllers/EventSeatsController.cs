using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    [Authorize(Roles = "admin, event manager")]
    [ResponseCache(CacheProfileName = "Caching")]
    public class EventSeatsController : Controller
    {
        private readonly IService<EventSeatDto> _service;

        public EventSeatsController(IService<EventSeatDto> service)
        {
            _service = service;
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

            var eventSeat = await _service.GetByIdAsync((int)id);
            if (eventSeat == null)
            {
                return NotFound();
            }

            return View(eventSeat);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventSeatDto eventSeat)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(eventSeat);
                return RedirectToAction(nameof(Index));
            }

            return View(eventSeat);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingEventSeat = await _service.GetByIdAsync((int)id);
            if (updatingEventSeat == null)
            {
                return NotFound();
            }

            return View(updatingEventSeat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventSeatDto eventSeat)
        {
            if (id != eventSeat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(eventSeat);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EventSeatExists(eventSeat.Id))
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

            return View(eventSeat);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingEventSeat = await _service.GetByIdAsync((int)id);
            if (deletingEventSeat == null)
            {
                return NotFound();
            }

            return View(deletingEventSeat);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventSeat = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(eventSeat);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EventSeatExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
