using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.Web.Controllers
{
    public class SeatsController : Controller
    {
        private readonly IService<SeatDto> _service;

        public SeatsController(IService<SeatDto> service)
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

            var seat = await _service.GetByIdAsync((int)id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AreaId,Row,Number")] SeatDto seat)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(seat);
                return RedirectToAction(nameof(Index));
            }

            return View(seat);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var updatingSeat = await _service.GetByIdAsync((int)id);
            if (updatingSeat == null)
            {
                return NotFound();
            }

            return View(updatingSeat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AreaId,Row,Number")] SeatDto seat)
        {
            if (id != seat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(seat);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SeatExists(seat.Id))
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

            return View(seat);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deletingSeat = await _service.GetByIdAsync((int)id);
            if (deletingSeat == null)
            {
                return NotFound();
            }

            return View(deletingSeat);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seat = await _service.GetByIdAsync(id);
            await _service.DeleteAsync(seat);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SeatExists(int id)
        {
            return await _service.GetByIdAsync(id) is not null;
        }
    }
}
