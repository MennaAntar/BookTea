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
    public class PublishingHousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublishingHousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PublishingHouses
        public async Task<IActionResult> Index(string term, string orderby, int CurrentPage = 1)
        {
            var publishingHouses = await _context.PublishingHouses.ToListAsync();
            //Search
            if (!String.IsNullOrEmpty(term))
            {
                publishingHouses = publishingHouses.Where(p =>
                    p.Id.ToString().Contains(term) ||
                    p.Country.Contains(term) ||
                    p.Name.Contains(term)).ToList();
            }

            // Sort
            ViewBag.OrderName = orderby == "Name" ? "Name_des" : "Name";
            ViewBag.OrderCountry = orderby == "Country" ? "Country_des" : "Country";
            ViewBag.OrderId = orderby == "Id" ? "Id_des" : "Id";

            switch (orderby)
            {
                case "Name":
                    publishingHouses = publishingHouses.OrderBy(p => p.Name).ToList();
                    break;
                case "Name_des":
                    publishingHouses = publishingHouses.OrderByDescending(p => p.Name).ToList();
                    break;
                case "Country":
                    publishingHouses = publishingHouses.OrderBy(p => p.Country).ToList();
                    break;
                case "Country_des":
                    publishingHouses = publishingHouses.OrderByDescending(p => p.Country).ToList();
                    break;
                case "Id":
                    publishingHouses = publishingHouses.OrderBy(p => p.Id).ToList();
                    break;
                case "Id_des":
                    publishingHouses = publishingHouses.OrderByDescending(p => p.Id).ToList();
                    break;
                default:
                    publishingHouses = publishingHouses.OrderBy(p => p.Id).ToList();
                    break;
            }

            //Pagination
            const int PageSize = 5;
            int TotalRecords = publishingHouses.Count;
            int NumPages = (int)Math.Ceiling(Convert.ToDecimal(TotalRecords / (decimal)PageSize));
            ViewBag.NumPages = NumPages;
            ViewBag.CurrentPage = CurrentPage;
            ViewBag.TotalRecords = TotalRecords;
            publishingHouses = publishingHouses.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            return View(publishingHouses);
        }
        // GET: PublishingHouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PublishingHouses == null)
            {
                return NotFound();
            }

            var publishingHouse = await _context.PublishingHouses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publishingHouse == null)
            {
                return NotFound();
            }

            return View(publishingHouse);
        }

        // GET: PublishingHouses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PublishingHouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country")] PublishingHouse publishingHouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publishingHouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publishingHouse);
        }

        // GET: PublishingHouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PublishingHouses == null)
            {
                return NotFound();
            }

            var publishingHouse = await _context.PublishingHouses.FindAsync(id);
            if (publishingHouse == null)
            {
                return NotFound();
            }
            return View(publishingHouse);
        }

        // POST: PublishingHouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country")] PublishingHouse publishingHouse)
        {
            if (id != publishingHouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publishingHouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublishingHouseExists(publishingHouse.Id))
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
            return View(publishingHouse);
        }

        // GET: PublishingHouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PublishingHouses == null)
            {
                return NotFound();
            }

            var publishingHouse = await _context.PublishingHouses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publishingHouse == null)
            {
                return NotFound();
            }

            return View(publishingHouse);
        }

        // POST: PublishingHouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PublishingHouses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PublishingHouses'  is null.");
            }
            var publishingHouse = await _context.PublishingHouses.FindAsync(id);
            if (publishingHouse != null)
            {
                _context.PublishingHouses.Remove(publishingHouse);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublishingHouseExists(int id)
        {
          return (_context.PublishingHouses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
