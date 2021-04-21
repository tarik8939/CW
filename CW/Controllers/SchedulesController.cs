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
    public class SchedulesController : Controller
    {
        private readonly CWContext _context;

        public SchedulesController(CWContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var cWContext = _context.Schedules
                .Include(s => s.Route)
                .Include(s => s.Transport)
                .Include(s => s.Worker);
            return View(await cWContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Route)
                .Include(s => s.Transport)
                .ThenInclude(x=>x.Brand)
                .Include(s => s.Worker)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId");
            ViewData["TransportId"] = new SelectList(_context.Transports, "TransportId", "Description");
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Email");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleId,WorkerId,RouteId,TransportId,StartDateTime,EndDateTime,DateAdded,DateUpdated")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId", schedule.RouteId);
            ViewData["TransportId"] = new SelectList(_context.Transports, "TransportId", "Description", schedule.TransportId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Email", schedule.WorkerId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId", schedule.RouteId);
            ViewData["TransportId"] = new SelectList(_context.Transports, "TransportId", "Description", schedule.TransportId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Email", schedule.WorkerId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,WorkerId,RouteId,TransportId,StartDateTime,EndDateTime,DateAdded,DateUpdated")] Schedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.ScheduleId))
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
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "RouteId", schedule.RouteId);
            ViewData["TransportId"] = new SelectList(_context.Transports, "TransportId", "Description", schedule.TransportId);
            ViewData["WorkerId"] = new SelectList(_context.Workers, "WorkerId", "Email", schedule.WorkerId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Route)
                .Include(s => s.Transport)
                .Include(s => s.Worker)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.ScheduleId == id);
        }

        //список маршрутів для певного водія
        public async Task<IActionResult> GetSheduleForWorker(string email)
        {
            //var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Email == email);
            //var routes = await _context.Workers.Include(x => x.Schedules).
            //    Where(x => x.Email == email).ToListAsync();
            //var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Email == email);
            //var route = await _context.Schedules.Where(x => x.WorkerId == worker.WorkerId).ToListAsync();
            var route2 = await _context.Schedules.Where(x => x.Worker.Email == email)
                .Include(x=>x.Route)
                .ThenInclude(x=>x.CityFromNavigation)
                .Include(x => x.Route)
                .ThenInclude(x => x.CityToNavigation)
                .Include(x=>x.Transport)
                .ToListAsync();

            return View("../Schedules/Schedules", route2);
        }
    }
}
