using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CW.Models;
using Microsoft.AspNetCore.Authorization;

namespace CW.Controllers
{
    public class WorkersController : Controller
    {
        private readonly CWContext _context;

        public WorkersController(CWContext context)
        {
            _context = context;
        }

        // GET: Workers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var cWContext = _context.Workers
                .Include(w => w.Role)
                .Where(x => x.Role.Role1 != "Admin");
            return View(await cWContext.ToListAsync());
        }

        // GET: Workers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(w => w.Role)
                .FirstOrDefaultAsync(m => m.WorkerId == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(x=>x.Role1!="Admin"), "RoleId", "Role1");
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Worker worker)
        {
            if (ModelState.IsValid)
            {
                worker.DateAdded = DateTime.Now;
                worker.DateUpdated = DateTime.Now;
                //worker.WorkerId = _context.Workers.Count()+1;
                worker.WorkerId = _context.Workers.Max(x=>x.WorkerId)+1;
                _context.Workers.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(x => x.Role1 != "Admin"), "RoleId", "Role1", worker.RoleId);
            return View(worker);
        }

        // GET: Workers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(x => x.Role1 != "Admin"), "RoleId", "Role1", worker.RoleId);
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Worker worker)
        {
            if (id != worker.WorkerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    worker.DateUpdated = DateTime.Now;
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.WorkerId))
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
            ViewData["RoleId"] = new SelectList(_context.Roles.Where(x => x.Role1 != "Admin"), "RoleId", "Role1", worker.RoleId);
            return View(worker);
        }

        // GET: Workers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Workers
                .Include(w => w.Role)
                .FirstOrDefaultAsync(m => m.WorkerId == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.WorkerId == id);
        }

    }
}
