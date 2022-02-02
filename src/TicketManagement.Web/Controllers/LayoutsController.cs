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
    public class LayoutsController : Controller
    {
        private readonly TicketManagementContext _context;

        public LayoutsController(TicketManagementContext context)
        {
            _context = context;
        }

        // GET: Layouts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Layouts.ToListAsync());
        }

        // GET: Layouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var layout = await _context.Layouts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (layout == null)
            {
                return NotFound();
            }

            return View(layout);
        }

        // GET: Layouts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Layouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VenueId,Description,Name")] Layout layout)
        {
            if (ModelState.IsValid)
            {
                _context.Add(layout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(layout);
        }

        // GET: Layouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var layout = await _context.Layouts.FindAsync(id);
            if (layout == null)
            {
                return NotFound();
            }

            return View(layout);
        }

        // POST: Layouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VenueId,Description,Name")] Layout layout)
        {
            if (id != layout.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(layout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LayoutExists(layout.Id))
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

            return View(layout);
        }

        // GET: Layouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var layout = await _context.Layouts
                .SingleOrDefaultAsync(m => m.Id == id);
            if (layout == null)
            {
                return NotFound();
            }

            return View(layout);
        }

        // POST: Layouts/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var layout = await _context.Layouts.FindAsync(id);
            _context.Layouts.Remove(layout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LayoutExists(int id)
        {
            return _context.Layouts.Any(e => e.Id == id);
        }
    }
}
