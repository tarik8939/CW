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
    public class TransportsController : Controller
    {
        private readonly CWContext _context;

        public TransportsController(CWContext context)
        {
            _context = context;
        }

        // GET: Transports
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var cWContext = _context.Transports
                .Include(t => t.Brand)
                .Include(t => t.BusType);
            return View(await cWContext.ToListAsync());
        }

        // GET: Transports/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _context.Transports
                .Include(t => t.Brand)
                .Include(t => t.BusType)
                .FirstOrDefaultAsync(m => m.TransportId == id);
            if (transport == null)
            {
                return NotFound();
            }

            return View(transport);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBrand()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBrand(Brand brand)
        {
            if (ModelState.IsValid)
            {
                brand.DateAdded = DateTime.Now;
                brand.DateUpdated = DateTime.Now;
                _context.Add(brand);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Create");
            }
            return View(brand);
        }

        // GET: Transports/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["BusTypeId"] = new SelectList(_context.BusTypes, "BusTypeId", "TypeName");
            return View();
        }

        // POST: Transports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Transport transport)
        {
            if (ModelState.IsValid)
            {
                transport.DateAdded = DateTime.Now;
                transport.DateUpdated = DateTime.Now;
                _context.Add(transport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", transport.BrandId);
            ViewData["BusTypeId"] = new SelectList(_context.BusTypes, "BusTypeId", "TypeName", transport.BusTypeId);
            return View(transport);
        }

        // GET: Transports/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _context.Transports.FindAsync(id);
            if (transport == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", transport.BrandId);
            ViewData["BusTypeId"] = new SelectList(_context.BusTypes, "BusTypeId", "TypeName", transport.BusTypeId);
            return View(transport);
        }

        // POST: Transports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Transport transport)
        {
            if (id != transport.TransportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    transport.DateUpdated = DateTime.Now;
                    _context.Update(transport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransportExists(transport.TransportId))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", transport.BrandId);
            ViewData["BusTypeId"] = new SelectList(_context.BusTypes, "BusTypeId", "TypeName", transport.BusTypeId);
            return View(transport);
        }

        // GET: Transports/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transport = await _context.Transports
                .Include(t => t.Brand)
                .Include(t => t.BusType)
                .FirstOrDefaultAsync(m => m.TransportId == id);
            if (transport == null)
            {
                return NotFound();
            }

            return View(transport);
        }

        // POST: Transports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transport = await _context.Transports.FindAsync(id);
            _context.Transports.Remove(transport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransportExists(int id)
        {
            return _context.Transports.Any(e => e.TransportId == id);
        }
    }
}
