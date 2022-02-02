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
    public class EventAreasController : Controller
    {
        private readonly TicketManagementContext _context;

        public EventAreasController(TicketManagementContext context)
        {
            _context = context;
        }

        // GET: EventAreas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventAreas.ToListAsync());
        }

        // GET: EventAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventArea = await _context.EventAreas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventArea == null)
            {
                return NotFound();
            }

            return View(eventArea);
        }

        // GET: EventAreas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,Description,CoordX,CoordY,Price")] EventArea eventArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(eventArea);
        }

        // GET: EventAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventArea = await _context.EventAreas.FindAsync(id);
            if (eventArea == null)
            {
                return NotFound();
            }

            return View(eventArea);
        }

        // POST: EventAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,Description,CoordX,CoordY,Price")] EventArea eventArea)
        {
            if (id != eventArea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventAreaExists(eventArea.Id))
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

            return View(eventArea);
        }

        // GET: EventAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventArea = await _context.EventAreas
                .SingleOrDefaultAsync(m => m.Id == id);
            if (eventArea == null)
            {
                return NotFound();
            }

            return View(eventArea);
        }

        // POST: EventAreas/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventArea = await _context.EventAreas.FindAsync(id);
            _context.EventAreas.Remove(eventArea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventAreaExists(int id)
        {
            return _context.EventAreas.Any(e => e.Id == id);
        }
    }
}
