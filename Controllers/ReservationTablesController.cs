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

    [Authorize]

    public class ReservationTablesController : Controller
    {
        private readonly DataContext _context;

        public ReservationTablesController(DataContext context)
        {
            _context = context;
        }

        // GET: ReservationTables
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = (User?)HttpContext.Items["User"];
            var dataContext = _context.Reservations.Include(r => r.Table).Include(r => r.User);
            if (user.Role != Enums.Role.ADMIN)
            {
                return View(await dataContext.Where(x=>x.UserId == user.Id).ToListAsync());
            }
            return View(await dataContext.ToListAsync());

        }

        // GET: ReservationTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationTables = await _context.Reservations
                .Include(r => r.Table)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationTables == null)
            {
                return NotFound();
            }

            return View(reservationTables);
        }

        // GET: ReservationTables/Create
        
        public IActionResult Create()
        {
            ViewData["TableId"] = new SelectList(_context.ReserveTables, "Id", "Id");
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ReservationTables reservationTables)
        {
            var user = (User?)HttpContext.Items["User"];
            var exist = _context.Reservations.OrderByDescending(x=>x.ReserveDate).Where(x => x.TableId == reservationTables.TableId);
            if(exist == null)
            {
                reservationTables.UserId = user.Id;
                _context.Add(reservationTables);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            foreach (var reserve in exist) {
                if (reserve.ReserveDate.AddHours(1.5) >= reservationTables.ReserveDate && reserve.ReserveDate <= reservationTables.ReserveDate.AddHours(1.5))
                {
                    ModelState.AddModelError(string.Empty, "This Table was reserved");
                    return View();
                } 
            }
                reservationTables.UserId = user.Id;
                _context.Add(reservationTables);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

        }

        // GET: ReservationTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationTables = await _context.Reservations.FindAsync(id);
            if (reservationTables == null)
            {
                return NotFound();
            }
            ViewData["TableId"] = new SelectList(_context.ReserveTables, "Id", "Id", reservationTables.TableId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", reservationTables.UserId);
            return View(reservationTables);
        }

        // POST: ReservationTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TableId,ReserveDate")] ReservationTables reservationTables)
        {
            if (id != reservationTables.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationTables);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationTablesExists(reservationTables.Id))
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
            ViewData["TableId"] = new SelectList(_context.ReserveTables, "Id", "Id", reservationTables.TableId);
            return View(reservationTables);
        }

        // GET: ReservationTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationTables = await _context.Reservations
                .Include(r => r.Table)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservationTables == null)
            {
                return NotFound();
            }

            return View(reservationTables);
        }

        // POST: ReservationTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservationTables = await _context.Reservations.FindAsync(id);
            if (reservationTables != null)
            {
                _context.Reservations.Remove(reservationTables);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationTablesExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
