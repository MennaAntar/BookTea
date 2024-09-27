﻿using System;
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
        public async Task<IActionResult> Index(string term, string orderby = "Id", int CurrentPage = 1)
        {
            //Search
            var shipingCompany = await _context.ShippingCompanies.ToListAsync();
            if (!String.IsNullOrEmpty(term))
            {
                shipingCompany = shipingCompany.Where(a => a.Cost.ToString().Contains(term)
                || a.Date.ToString().Contains(term) || a.Weight.ToString().Contains(term)
                || a.Destination.Contains(term)).ToList();

            }

            //Sort
            ViewBag.OrderCost = orderby == "Cost" ? "Cost_des" : "Cost";
            ViewBag.OrderDate = orderby == "Date" ? "Date_des" : "Date";
            ViewBag.OrderWeight = orderby == "Weight" ? "Weight_des" : "Weight";
            ViewBag.OrderDestination = orderby == "Destination" ? "Destination_des" : "Destination";
            switch (orderby)
            {
                case "Cost":
                    shipingCompany = shipingCompany.OrderBy(a => a.Cost).ToList();
                    break;
                case "Cost_des":
                    shipingCompany = shipingCompany.OrderByDescending(a => a.Cost).ToList();
                    break;
                case "Date":
                    shipingCompany = shipingCompany.OrderBy(a => a.Date).ToList();
                    break;
                case "Date_des":
                    shipingCompany = shipingCompany.OrderByDescending(a => a.Date).ToList();
                    break;
                case "Weight":
                    shipingCompany = shipingCompany.OrderBy(a => a.Weight).ToList();
                    break;
                case "Weight_des":
                    shipingCompany = shipingCompany.OrderByDescending(a => a.Weight).ToList();
                    break;
                case "Destination":
                    shipingCompany = shipingCompany.OrderBy(a => a.Destination).ToList();
                    break;
                case "Destination_des":
                    shipingCompany = shipingCompany.OrderByDescending(a => a.Destination).ToList();
                    break;
                default:
                    shipingCompany = shipingCompany.OrderBy(a => a.Id).ToList();
                    break;
            }

            //Pagination
            const int PageSize = 5;
            int TotalRecords = shipingCompany.Count;
            int NumPages = (int)Math.Ceiling(Convert.ToDecimal(TotalRecords / (decimal)PageSize));
            ViewBag.NumPages = NumPages;
            ViewBag.CurrentPage = CurrentPage;
            ViewBag.TotalRecords = TotalRecords;
            shipingCompany = shipingCompany.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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
