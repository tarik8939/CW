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
            var cWContext = _context.Routes
                .Include(r => r.CityFromNavigation)
                .Include(r => r.CityToNavigation);
            return View(await cWContext.ToListAsync());
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Cashier-Intern,Cashier")]
        public IActionResult Search()
        {
            ViewData["CityFrom"] = new SelectList(_context.Cities, "CityId", "City1");
            ViewData["CityTo"] = new SelectList(_context.Cities, "CityId", "City1");
            return View();
        }
        //[HttpPost("{CityFrom}/{СityTo}")]
        //public async Task<IActionResult> FindRoute(string CityFrom, string СityTo)
        [Authorize(Roles = "Admin,Cashier-Intern,Cashier")]
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
                    .ThenInclude(x => x.CityFromNavigation)
                .Include(x => x.Route)
                    .ThenInclude(x => x.CityToNavigation)
                .Include(x => x.Transport)
                    .ThenInclude(x => x.Brand)
                .ToListAsync();
            return View("../Schedules/Index", schedule);
        }

        [Authorize(Roles = "Admin,Cashier-Intern,Cashier")]
        public async Task<IActionResult> GetSchedules(int id)
        {
            var list = await _context.Schedules
                .Include(x=>x.Transport)
                .ThenInclude(x=>x.Brand)
                .Include(x=>x.Worker)
                .Where(x => x.RouteId == id).ToListAsync();

            return View( list);
        }

        // GET: Routes/Create
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Create(Route route)
        {
            var cityfrom = _context.Cities.FirstOrDefault(x => x.CityId == route.CityFrom);
            var cityto = _context.Cities.FirstOrDefault(x => x.CityId == route.CityTo);
            route.Mileage = (int?) CalcDist(cityfrom, cityto);
            if (ModelState.IsValid)
            {
                route.DateAdded = DateTime.Now;
                route.DateUpdated = DateTime.Now;
                _context.Add(route);
                await _context.SaveChangesAsync();
                var r = _context.Routes.ToList().Last();
                AddStoping(r);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityFrom"] = new SelectList(_context.Cities, "CityId", "City1", route.CityFrom);
            ViewData["CityTo"] = new SelectList(_context.Cities, "CityId", "City1", route.CityTo);
            return View(route);
        }


        public double CalcDist(City cityfrom, City cityto)
        {
            const double R = 6371;
            double sin1 = Math.Sin((double) ((cityfrom.latitude - cityto.latitude) / 2));
            double sin2 = Math.Sin((double) ((cityfrom.longitude - cityto.longitude) / 2));
 
            return (2*R*Math.Cos(Math.Acos(sin1*sin1+sin2*sin2*Math.Cos((double)cityfrom.latitude) * Math.Cos((double)cityto.latitude))))/17;
        }

        // GET: Routes/Edit/5
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Edit(int id, Route route)
        {
            if (id != route.RouteId)
            {
                return NotFound();
            }
            var cityfrom = _context.Cities.FirstOrDefault(x => x.CityId == route.CityFrom);
            var cityto = _context.Cities.FirstOrDefault(x => x.CityId == route.CityTo);
            route.Mileage = (int?)CalcDist(cityfrom, cityto);
            if (ModelState.IsValid)
            {
                try
                {
                    route.DateUpdated = DateTime.Now;
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

        public async Task<IActionResult> Details(int id)
        {
            var item = _context.Routes
                .Include(x => x.RouteStops)
                .ThenInclude(x => x.City)
                .Include(x => x.CityFromNavigation)
                .Include(x => x.CityToNavigation)
                .FirstOrDefault(x => x.RouteId == id);
            return View(item);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Cashier-Intern,Cashier")]
        public async Task<IActionResult> SeeStops(int id)
        {
            var items = await _context.RouteStops
                .Include(x => x.City)
                .Where(x=>x.RouteId == id).OrderBy(x=>x.DistanceToStop).ToListAsync();
            return View("SeeStops", items);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStop(int id)
        {
            ViewData["City"] = new SelectList(_context.Cities, "CityId", "City1");
            var obj = new RouteStop();
            obj.RouteId = id;
            return View(obj);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStop(RouteStop stop)
        {
            stop.DateAdded = DateTime.Now;
            stop.DateUpdated = DateTime.Now;
            var r = _context.Routes.FirstOrDefault(x => x.RouteId == stop.RouteId);
            var cityfrom = _context.Cities.FirstOrDefault(x => x.CityId == r.CityFrom);
            var cityto = _context.Cities.FirstOrDefault(x => x.CityId == stop.CityId);
            stop.DistanceToStop = CalcDist(cityfrom, cityto);
            _context.RouteStops.Add(stop);
            await _context.SaveChangesAsync();
            ViewData["City"] = new SelectList(_context.Cities, "CityId", "City1");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async void AddStoping(Route route)
        {
            var city1 = _context.Cities.FirstOrDefault(x => x.CityId == route.CityFrom);
            var city2 = _context.Cities.FirstOrDefault(x => x.CityId == route.CityTo);

            var stop1 = new RouteStop();
            stop1.CityId = city1.CityId;
            stop1.RouteId = route.RouteId;
            stop1.DistanceToStop = CalcDist(city1, city1);
            
            var stop2 = new RouteStop();
            stop2.CityId = city2.CityId;
            stop2.RouteId = route.RouteId;
            stop2.DistanceToStop = CalcDist(city1, city2);
            _context.RouteStops.Add(stop1);
            _context.RouteStops.Add(stop2);
            await _context.SaveChangesAsync();
        }


        // GET: Routes/Delete/5
        [Authorize(Roles = "Admin")]
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
