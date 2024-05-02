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
    public class OrderLinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderLinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderLines
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OrderLines.Include(o => o.Book).Include(o => o.Order);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OrderLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderLines == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLines
                .Include(o => o.Book)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OL_Id == id);
            if (orderLine == null)
            {
                return NotFound();
            }

            return View(orderLine);
        }

        // GET: OrderLines/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "ISBN");
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            return View();
        }

        // POST: OrderLines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OL_Id,TotalProductPrice,ProductQuantityRequired,BookId,OrderId")] OrderLine orderLine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderLine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "ISBN", orderLine.BookId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderLine.OrderId);
            return View(orderLine);
        }

        // GET: OrderLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderLines == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLines.FindAsync(id);
            if (orderLine == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "ISBN", orderLine.BookId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderLine.OrderId);
            return View(orderLine);
        }

        // POST: OrderLines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OL_Id,TotalProductPrice,ProductQuantityRequired,BookId,OrderId")] OrderLine orderLine)
        {
            if (id != orderLine.OL_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderLineExists(orderLine.OL_Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "ISBN", orderLine.BookId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderLine.OrderId);
            return View(orderLine);
        }

        // GET: OrderLines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderLines == null)
            {
                return NotFound();
            }

            var orderLine = await _context.OrderLines
                .Include(o => o.Book)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.OL_Id == id);
            if (orderLine == null)
            {
                return NotFound();
            }

            return View(orderLine);
        }

        // POST: OrderLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderLines == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OrderLines'  is null.");
            }
            var orderLine = await _context.OrderLines.FindAsync(id);
            if (orderLine != null)
            {
                _context.OrderLines.Remove(orderLine);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderLineExists(int id)
        {
          return (_context.OrderLines?.Any(e => e.OL_Id == id)).GetValueOrDefault();
        }
    }
}
