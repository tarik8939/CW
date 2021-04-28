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
                .Include(s => s.Route).
                    ThenInclude(x=>x.CityFromNavigation)
                .Include(s => s.Route)
                    .ThenInclude(x => x.CityToNavigation)
                .Include(s => s.Transport)
                .ThenInclude(x=>x.Brand)
                .Include(s => s.Worker);
            return View(await cWContext.ToListAsync());
        }

        //список маршрутів для певного водія
        [Authorize(Roles = "Admin,Driver-Intern,Driver")]
        public async Task<IActionResult> GetSheduleForWorker(string email)
        {
            var route2 = await _context.Schedules.Where(x => x.Worker.Email == email)
                .Include(x => x.Route)
                    .ThenInclude(x => x.CityFromNavigation)
                .Include(x => x.Route)
                    .ThenInclude(x => x.CityToNavigation)
                .Include(x => x.Transport)
                    .ThenInclude(x => x.Brand)
                .Include(x=>x.Worker)
                .ToListAsync();

            return View("../Schedules/Schedules", route2);
        }

        // GET: Schedules/Details/5
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Route)
                    .ThenInclude(x => x.CityFromNavigation)
                .Include(s => s.Route)
                    .ThenInclude(x => x.CityToNavigation)
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
        public IActionResult Create(Route? route)
        {
            ViewData["Route"] = new SelectList(_context.Routes
                .Include(x=>x.CityFromNavigation)
                .Include(x=>x.CityToNavigation), "RouteId", "RouteName",route.RouteId);
            ViewData["Transport"] = new SelectList(_context.Transports
                .Include(x=>x.Brand), "TransportId", "TransportName");
            ViewData["Worker"] = new SelectList(_context.Workers.Include(x=>x.Role)
                    .Where(x=>x.Role.Role1.Contains("Driver")),"WorkerId", "WorkerFullName");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Schedule schedule)
        {
            var r = _context.Routes.Include(x => x.CityFromNavigation)
                .Include(x => x.CityToNavigation)
                .FirstOrDefault(x => x.RouteId == schedule.RouteId);
            var dist = CalcDist(r.CityFromNavigation, r.CityToNavigation);
            schedule.EndDateTime = CalcTime(schedule, dist);
            if (ModelState.IsValid)
            {
                schedule.DateAdded = DateTime.Now;
                schedule.DateUpdated = DateTime.Now;
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Route"] = new SelectList(_context.Routes
                .Include(x => x.CityFromNavigation)
                .Include(x => x.CityToNavigation), "RouteId", "RouteName", schedule.RouteId);
            ViewData["Transport"] = new SelectList(_context.Transports
                .Include(x => x.Brand), "TransportId", "TransportName", schedule.TransportId);
            ViewData["Worker"] = new SelectList(_context.Workers.Include(x => x.Role)
                .Where(x => x.Role.Role1.Contains("Driver")), "WorkerId", "WorkerFullName", schedule.WorkerId);

            return View(schedule);
        }

        [Authorize(Roles = "Admin")]
        private DateTime CalcTime(Schedule schedule, double dist)
        {
            DateTime date = schedule.StartDateTime;
            var time = Math.Ceiling(Convert.ToDouble(dist / 60.0));
            date = date.AddHours(time);
            return date;
        }

        public double CalcDist(City cityfrom, City cityto)
        {
            const double R = 6371;
            double sin1 = Math.Sin((double)((cityfrom.latitude - cityto.latitude) / 2));
            double sin2 = Math.Sin((double)((cityfrom.longitude - cityto.longitude) / 2));
            return 2 * R * Math.Asin(Math.Sqrt(sin1 * sin1 + sin2 * sin2 * Math.Cos((double)cityfrom.latitude) * Math.Cos((double)cityto.latitude)));
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
            ViewData["Transport"] = new SelectList(_context.Transports
                .Include(x => x.Brand), "TransportId", "TransportName", schedule.TransportId);
            ViewData["Worker"] = new SelectList(_context.Workers.Include(x => x.Role)
                .Where(x => x.Role.Role1.Contains("Driver")), "WorkerId", "WorkerFullName", schedule.WorkerId);

            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Schedule schedule)
        {
            if (id != schedule.ScheduleId)
            {
                return NotFound();
            }

            var s = _context.Schedules.FirstOrDefault(x => x.ScheduleId == id);
            var r = _context.Routes.Include(x => x.CityFromNavigation)
                .Include(x => x.CityToNavigation)
                .FirstOrDefault(x => x.RouteId == s.RouteId);
            var dist = CalcDist(r.CityFromNavigation, r.CityToNavigation);
            schedule.EndDateTime = CalcTime(schedule, dist);
            if (ModelState.IsValid)
            {
                try
                {
                    schedule.DateUpdated = DateTime.Now;
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
            ViewData["Transport"] = new SelectList(_context.Transports
                .Include(x => x.Brand), "TransportId", "TransportName", schedule.TransportId);
            ViewData["Worker"] = new SelectList(_context.Workers.Include(x => x.Role)
                .Where(x => x.Role.Role1.Contains("Driver")), "WorkerId", "WorkerFullName", schedule.WorkerId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Route)
                .ThenInclude(x => x.CityFromNavigation)
                .Include(s => s.Route)
                .ThenInclude(x => x.CityToNavigation)
                .Include(s => s.Transport)
                .ThenInclude(x => x.Brand)
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


    }
}
