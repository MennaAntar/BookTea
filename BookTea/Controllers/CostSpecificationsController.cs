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
    public class CostSpecificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CostSpecificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CostSpecifications
        public async Task<IActionResult> Index(string term, string orderby = "Id", int CurrentPage=1)
        {
            var costSpecification = await _context.CostsSpecifications.Include(c => c.ShippingCompany).ToListAsync();
            //Search
            if (!String.IsNullOrEmpty(term))
            {
                costSpecification = costSpecification.Where(a => a.CityName.Contains(term)
                || a.DeliveryCost.ToString().Contains(term)
                || a.ShippingCompanyId.ToString().Contains(term)).ToList();

            }

            //Sort
            ViewBag.OrderCityName = orderby == "CityName" ? "CityName_des" : "CityName";
            ViewBag.OrderDeliveryCost = orderby == "DeliveryCost" ? "DeliveryCost_des" : "DeliveryCost";
            ViewBag.OrderShippingCompanyId = orderby == "ShippingCompanyId" ? "ShippingCompanyId_des" : "ShippingCompanyId";
            switch (orderby)
            {
                case "CityName":
                    costSpecification = costSpecification.OrderBy(a => a.CityName).ToList();
                    break;
                case "CityName_des":
                    costSpecification = costSpecification.OrderByDescending(a => a.CityName).ToList();
                    break;
                case "DeliveryCost":
                    costSpecification = costSpecification.OrderBy(a => a.DeliveryCost).ToList();
                    break;
                case "DeliveryCost_des":
                    costSpecification = costSpecification.OrderByDescending(a => a.DeliveryCost).ToList();
                    break;
                case "ShippingCompanyId":
                    costSpecification = costSpecification.OrderBy(a => a.ShippingCompanyId).ToList();
                    break;
                case "ShippingCompanyId_des":
                    costSpecification = costSpecification.OrderByDescending(a => a.ShippingCompanyId).ToList();
                    break;
                default:
                    costSpecification = costSpecification.OrderBy(a => a.Id).ToList();
                    break;
            }

            //Pagination
            const int PageSize = 5;
            int TotalRecords = costSpecification.Count;
            int NumPages = (int)Math.Ceiling(Convert.ToDecimal(TotalRecords / (decimal)PageSize));
            ViewBag.NumPages = NumPages;
            ViewBag.CurrentPage = CurrentPage;
            ViewBag.TotalRecords = TotalRecords;
            costSpecification = costSpecification.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            return View(costSpecification);
        }

        // GET: CostSpecifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CostsSpecifications == null)
            {
                return NotFound();
            }

            var costSpecification = await _context.CostsSpecifications
                .Include(c => c.ShippingCompany)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (costSpecification == null)
            {
                return NotFound();
            }

            return View(costSpecification);
        }

        // GET: CostSpecifications/Create
        public IActionResult Create()
        {
            ViewData["ShippingCompanyId"] = new SelectList(_context.ShippingCompanies, "Id", "Id");
            return View();
        }

        // POST: CostSpecifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CityName,DeliveryCost,ShippingCompanyId")] CostSpecification costSpecification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(costSpecification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShippingCompanyId"] = new SelectList(_context.ShippingCompanies, "Id", "Id", costSpecification.ShippingCompanyId);
            return View(costSpecification);
        }

        // GET: CostSpecifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CostsSpecifications == null)
            {
                return NotFound();
            }

            var costSpecification = await _context.CostsSpecifications.FindAsync(id);
            if (costSpecification == null)
            {
                return NotFound();
            }
            ViewData["ShippingCompanyId"] = new SelectList(_context.ShippingCompanies, "Id", "Id", costSpecification.ShippingCompanyId);
            return View(costSpecification);
        }

        // POST: CostSpecifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityName,DeliveryCost,ShippingCompanyId")] CostSpecification costSpecification)
        {
            if (id != costSpecification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(costSpecification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CostSpecificationExists(costSpecification.Id))
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
            ViewData["ShippingCompanyId"] = new SelectList(_context.ShippingCompanies, "Id", "Id", costSpecification.ShippingCompanyId);
            return View(costSpecification);
        }

        // GET: CostSpecifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CostsSpecifications == null)
            {
                return NotFound();
            }

            var costSpecification = await _context.CostsSpecifications
                .Include(c => c.ShippingCompany)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (costSpecification == null)
            {
                return NotFound();
            }

            return View(costSpecification);
        }

        // POST: CostSpecifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CostsSpecifications == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CostsSpecifications'  is null.");
            }
            var costSpecification = await _context.CostsSpecifications.FindAsync(id);
            if (costSpecification != null)
            {
                _context.CostsSpecifications.Remove(costSpecification);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CostSpecificationExists(int id)
        {
          return (_context.CostsSpecifications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
