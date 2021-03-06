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
    public class PurchasesController : Controller
    {
        private readonly CWContext _context;
        private static object tmpobj;

        public PurchasesController(CWContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            var cWContext = _context.Purchases
                .Include(p => p.Client)
                .Include(p => p.Department)
                    .ThenInclude(x=>x.City)
                .Include(p => p.Ticket)
                .ThenInclude(x=>x.RouteStopFromNavigation)
                .ThenInclude(x=>x.City)
                .Include(p => p.Ticket)
                .ThenInclude(x => x.RouteStopToNavigation)
                .ThenInclude(x=>x.City)
                .Include(p => p.Worker);
            return View(await cWContext.ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Cashier-Intern,Cashier")]
        public async Task<IActionResult> BuyTicket(int id)
        {
            var item = _context.Schedules.FirstOrDefault(x => x.ScheduleId == id);
            ViewData["RouteStopFrom"] = new SelectList(_context.RouteStops.Where(x=>x.RouteId==item.RouteId)
                .Include(x=>x.City)
                .OrderBy(x=>x.DistanceToStop), "StopId", "City.City1");
            ViewData["RouteStopTo"] = new SelectList(_context.RouteStops.Where(x => x.RouteId == item.RouteId)
                .Include(x => x.City)
                .OrderBy(x => x.DistanceToStop), "StopId", "City.City1");

            tmpobj = item;
            return View();
        }
        public double CalcDist(City cityfrom, City cityto)
        {
            const double R = 6371;
            double sin1 = Math.Sin((double)((cityfrom.latitude - cityto.latitude) / 2));
            double sin2 = Math.Sin((double)((cityfrom.longitude - cityto.longitude) / 2));
            return 2 * R * Math.Asin(Math.Sqrt(sin1 * sin1 + sin2 * sin2 * Math.Cos((double)cityfrom.latitude) * Math.Cos((double)cityto.latitude)));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Cashier-Intern,Cashier")]
        public ActionResult CalculatePrice(Ticket t)
        {
            var obj = _context.RouteStops.Include(x=>x.City)
                .Include(x=>x.Route).FirstOrDefault(x => x.StopId == t.RouteStopFrom);
            var obj2 = _context.RouteStops.Include(x => x.City)
                .Include(x => x.Route).FirstOrDefault(x => x.StopId == t.RouteStopTo);
            obj.DistanceToStop = CalcDist(obj.Route.CityFromNavigation, obj.City);
            obj2.DistanceToStop = CalcDist(obj2.Route.CityFromNavigation, obj2.City);
            var schedule = tmpobj as Schedule;
            var transport = _context.Transports.FirstOrDefault(x => x.TransportId == schedule.TransportId);
            DateTime startTime = CalcTime(schedule,obj);
            DateTime endTime = CalcTime(schedule, obj2);
            t.ScheduleId = schedule.ScheduleId;
            t.Price = (decimal) ((transport.PricePerKm * (decimal?) (obj2.DistanceToStop - obj.DistanceToStop)));
            if (t.Price>1000 && t.Price<10000)
            {
                t.Price /= 10;
            }
            else if(t.Price>10001)
            {
                t.Price /= 100;
            }
            t.RouteStopFrom = obj.StopId;
            t.RouteStopTo = obj2.StopId;
            t.StartTime = startTime;
            t.ArrivalTime = endTime;
            t.DateAdded = DateTime.Now;
            t.DateUpdated = DateTime.Now;
            _context.Add(t);
            _context.SaveChanges();
            var tc = _context.Tickets.ToList().Last();

            return RedirectToAction("Create", tc);

        }

        [Authorize(Roles = "Admin,Cashier-Intern,Cashier")]
        private DateTime CalcTime(Schedule schedule, RouteStop? routeStop)
        {
            DateTime date =schedule.StartDateTime;
            var time = Math.Ceiling(Convert.ToDouble(routeStop.DistanceToStop / 60.0));
            if (time>150)
            {
                time /= 15;
            }
            date = date.AddHours(time);
            var a = 6;
            return date;
        }


        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Client)
                .Include(p => p.Department)
                .ThenInclude(x => x.City)
                .Include(p => p.Ticket)
                .ThenInclude(x => x.RouteStopFromNavigation)
                .ThenInclude(x => x.City)
                .Include(p => p.Ticket)
                .ThenInclude(x => x.RouteStopToNavigation)
                .ThenInclude(x => x.City)
                .Include(p => p.Worker)
                .FirstOrDefaultAsync(m => m.PurchaseId == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create(Ticket ticket)
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments.Include(x=>x.City), "DeptId", "Fulladdress");
            ViewData["TicketPrice"] = Convert.ToInt32(ticket.Price);
            ViewData["TicketID"] = ticket.TicketId;
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Purchase purchase)
        {
            var Clieent = new Client(purchase.Client.FirstName, purchase.Client.LastName);
            if (Clieent.FirstName==null || Clieent.LastName==null)
            {
                ViewData["DepartmentId"] = new SelectList(_context.Departments.Include(x => x.City), "DeptId", "Fulladdress");
                return View(purchase);
            }
            else
            {
                Clieent.DateAdded = DateTime.Now;
                Clieent.DateUpdated = DateTime.Now;
                _context.Add(Clieent);
                _context.SaveChanges();
                Clieent = _context.Clients.ToList().Last();
                purchase.ClientId = Clieent.ClientId;
            }

            var worker = _context.Workers.FirstOrDefault(x => x.Email == @User.Identity.Name);
            purchase.WorkerId = worker.WorkerId;
            purchase.TicketId =  _context.Tickets.ToList().Last().TicketId;
            var asd = 3;
            if (ModelState.IsValid)
            {
                purchase.DateAdded = DateTime.Now;
                purchase.DateUpdated = DateTime.Now;
                purchase.PurchaseDate = DateTime.Now;
                _context.Add(purchase);
                await _context.SaveChangesAsync();
                return Redirect("../Home/Index");
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments.Include(x => x.City), "DeptId", "Fulladdress");
            return View(purchase);
        }

        

        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.PurchaseId == id);
        }
    }
}
