using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.Web.Controllers
{
    public class EventSeatsController : Controller
    {
        private readonly TicketManagementContext _context;

        public EventSeatsController(TicketManagementContext context)
        {
            _context = context;
        }

        // GET: EventSeats
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventSeats.ToListAsync());
        }

        // GET: EventSeats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSeat = await _context.EventSeats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventSeat == null)
            {
                return NotFound();
            }

            return View(eventSeat);
        }

        // GET: EventSeats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventSeats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventAreaId,Row,Number,State")] EventSeat eventSeat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventSeat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(eventSeat);
        }

        // GET: EventSeats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSeat = await _context.EventSeats.FindAsync(id);
            if (eventSeat == null)
            {
                return NotFound();
            }

            return View(eventSeat);
        }

        // POST: EventSeats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventAreaId,Row,Number,State")] EventSeat eventSeat)
        {
            if (id != eventSeat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventSeat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventSeatExists(eventSeat.Id))
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

        // GET: EventSeats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSeat = await _context.EventSeats
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eventSeat == null)
            {
                return NotFound();
            }

            return View(eventSeat);
        }

        // POST: EventSeats/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventSeat = await _context.EventSeats.FindAsync(id);
            _context.EventSeats.Remove(eventSeat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventSeatExists(int id)
        {
            return _context.EventSeats.Any(e => e.Id == id);
        }
    }
}
