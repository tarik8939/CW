using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CW.Models;

namespace CW.Controllers
{
    public class RoutesController : Controller
    {
        private readonly CWContext _context;

        public RoutesController(CWContext context)
        {
            _context = context;
        }

        // GET: Routes
        public async Task<IActionResult> Index()
        {
            var cWContext = _context.Routes.Include(r => r.CityFromNavigation).Include(r => r.CityToNavigation);
            return View(await cWContext.ToListAsync());
        }
        [HttpGet]
        public IActionResult Search()
        {
            ViewData["CityFrom"] = new SelectList(_context.Cities, "CityId", "City1");
            ViewData["CityTo"] = new SelectList(_context.Cities, "CityId", "City1");
            var list = _context.Cities.ToList();
            return View();
        }
        //[HttpPost("{CityFrom}/{СityTo}")]
        //public async Task<IActionResult> FindRoute(string CityFrom, string СityTo)
        public async Task<IActionResult> FindRoute(Route route)
        {
            var list = _context.Routes.Where(x => x.CityFrom == route.CityFrom).Where(x => x.CityTo == route.CityTo)
                .ToList();
            var r = _context.Routes.Where(x => x.CityFrom == route.CityFrom).Where(x => x.CityTo == route.CityTo)
                .FirstOrDefault(x => x.CityFrom == route.CityFrom);
            var test1 = _context.RouteStops.Where(x => x.RouteId == r.RouteId).ToList();

            var schedule = await _context.Schedules.Where(x => x.RouteId == r.RouteId)
                .Include(x=>x.Worker)
                .Include(x=>x.Route)
                    .ThenInclude(x=>x.CityFromNavigation)
                .Include(x => x.Route)
                    .ThenInclude(x => x.CityToNavigation)
                .Include(x=>x.Transport)
                    .ThenInclude(x=>x.Brand)
                .ToListAsync();

            return View("../Schedules/Index", schedule);
        }

        // GET: Routes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.CityFromNavigation)
                .Include(r => r.CityToNavigation)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // GET: Routes/Create
        public IActionResult Create()
        {
            ViewData["CityFrom"] = new SelectList(_context.Cities, "CityId", "City1");
            ViewData["CityTo"] = new SelectList(_context.Cities, "CityId", "City1");
            return View();
        }

        // POST: Routes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouteId,Mileage,CityFrom,CityTo,NumberOfRoute,StopCount,DateAdded,DateUpdated")] Route route)
        {
            if (ModelState.IsValid)
            {
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityFrom"] = new SelectList(_context.Cities, "CityId", "City1", route.CityFrom);
            ViewData["CityTo"] = new SelectList(_context.Cities, "CityId", "City1", route.CityTo);
            return View(route);
        }

        // GET: Routes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            ViewData["CityFrom"] = new SelectList(_context.Cities, "CityId", "City1", route.CityFrom);
            ViewData["CityTo"] = new SelectList(_context.Cities, "CityId", "City1", route.CityTo);
            return View(route);
        }

        // POST: Routes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouteId,Mileage,CityFrom,CityTo,NumberOfRoute,StopCount,DateAdded,DateUpdated")] Route route)
        {
            if (id != route.RouteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.RouteId))
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
            ViewData["CityFrom"] = new SelectList(_context.Cities, "CityId", "City1", route.CityFrom);
            ViewData["CityTo"] = new SelectList(_context.Cities, "CityId", "City1", route.CityTo);
            return View(route);
        }

        // GET: Routes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.CityFromNavigation)
                .Include(r => r.CityToNavigation)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.RouteId == id);
        }
    }
}
