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
    public class PublishingHousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PublishingHousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PublishingHouses
        public async Task<IActionResult> Index()
        {
              return _context.PublishingHouses != null ? 
                          View(await _context.PublishingHouses.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.PublishingHouses'  is null.");
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
