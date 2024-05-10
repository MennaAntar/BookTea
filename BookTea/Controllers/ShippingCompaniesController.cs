using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookTea.Context;
using BookTea.Models;

namespace BookTea.Controllers
{
    public class ShippingCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShippingCompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShippingCompanies
        public async Task<IActionResult> Index(string term)
        {
            var shipingCompany = await _context.ShippingCompanies.ToListAsync();
            if (!String.IsNullOrEmpty(term))
            {
                shipingCompany = shipingCompany.Where(a => a.Cost.ToString().Contains(term)
                || a.Date.ToString().Contains(term) || a.Weight.ToString().Contains(term) || a.Destination.Contains(term)).ToList();

            }

            return View(shipingCompany);
        }

        // GET: ShippingCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShippingCompanies == null)
            {
                return NotFound();
            }

            var shippingCompany = await _context.ShippingCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shippingCompany == null)
            {
                return NotFound();
            }

            return View(shippingCompany);
        }

        // GET: ShippingCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShippingCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cost,Date,Weight,Destination")] ShippingCompany shippingCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippingCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shippingCompany);
        }

        // GET: ShippingCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShippingCompanies == null)
            {
                return NotFound();
            }

            var shippingCompany = await _context.ShippingCompanies.FindAsync(id);
            if (shippingCompany == null)
            {
                return NotFound();
            }
            return View(shippingCompany);
        }

        // POST: ShippingCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cost,Date,Weight,Destination")] ShippingCompany shippingCompany)
        {
            if (id != shippingCompany.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingCompanyExists(shippingCompany.Id))
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
            return View(shippingCompany);
        }

        // GET: ShippingCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShippingCompanies == null)
            {
                return NotFound();
            }

            var shippingCompany = await _context.ShippingCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shippingCompany == null)
            {
                return NotFound();
            }

            return View(shippingCompany);
        }

        // POST: ShippingCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShippingCompanies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShippingCompanies'  is null.");
            }
            var shippingCompany = await _context.ShippingCompanies.FindAsync(id);
            if (shippingCompany != null)
            {
                _context.ShippingCompanies.Remove(shippingCompany);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippingCompanyExists(int id)
        {
          return (_context.ShippingCompanies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
