using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cafe.Data;
using Cafe.Models;

namespace Cafe.Controllers
{
    
    public class ReserveTablesController : Controller
    {
        private readonly DataContext _context;

        public ReserveTablesController(DataContext context)
        {
            _context = context;
        }

        // GET: ReserveTables
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReserveTables.ToListAsync());
        }

        // GET: ReserveTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserveTable = await _context.ReserveTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserveTable == null)
            {
                return NotFound();
            }

            return View(reserveTable);
        }

        // GET: 
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReserveTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Price,IsAvalible")] ReserveTable reserveTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserveTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reserveTable);
        }

        // GET: ReserveTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserveTable = await _context.ReserveTables.FindAsync(id);
            if (reserveTable == null)
            {
                return NotFound();
            }
            return View(reserveTable);
        }

        // POST: ReserveTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Price,IsAvalible")] ReserveTable reserveTable)
        {
            if (id != reserveTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserveTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReserveTableExists(reserveTable.Id))
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
            return View(reserveTable);
        }

        // GET: ReserveTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserveTable = await _context.ReserveTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserveTable == null)
            {
                return NotFound();
            }

            return View(reserveTable);
        }

        // POST: ReserveTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserveTable = await _context.ReserveTables.FindAsync(id);
            if (reserveTable != null)
            {
                _context.ReserveTables.Remove(reserveTable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReserveTableExists(int id)
        {
            return _context.ReserveTables.Any(e => e.Id == id);
        }
    }
}
