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
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index(string term, string orderby = "Id", int CurrentPage = 1)
        {
            var payments = await _context.Payments.Include(c => c.Order).ToListAsync();

            //Search
            if (!String.IsNullOrEmpty(term))
            {
                payments = payments.Where(a => a.Cost.ToString().Contains(term)
                || a.PendingDate.ToString().Contains(term) || a.Date.ToString().Contains(term) ||
                a.OrderId.ToString().Contains(term)).ToList();

            }

            //Sort
            ViewBag.OrderCost = orderby == "Cost" ? "Cost_des" : "Cost";
            ViewBag.OrderPendingDate = orderby == "PendingDate" ? "PendingDate_des" : "PendingDate";
            ViewBag.OrderDate = orderby == "Date" ? "Date_des" : "Date";
            ViewBag.OrderOrderId = orderby == "OrderId" ? "OrderId_des" : "OrderId";
            switch (orderby)
            {
                case "Cost":
                    payments = payments.OrderBy(a => a.Cost).ToList();
                    break;
                case "Cost_des":
                    payments = payments.OrderByDescending(a => a.Cost).ToList();
                    break;
                case "PendingDate":
                    payments = payments.OrderBy(a => a.PendingDate).ToList();
                    break;
                case "PendingDate_des":
                    payments = payments.OrderByDescending(a => a.PendingDate).ToList();
                    break;
                case "Date":
                    payments = payments.OrderBy(a => a.Date).ToList();
                    break;
                case "Date_des":
                    payments = payments.OrderByDescending(a => a.Date).ToList();
                    break;
                case "OrderId":
                    payments = payments.OrderBy(a => a.OrderId).ToList();
                    break;
                case "OrderId_des":
                    payments = payments.OrderByDescending(a => a.OrderId).ToList();
                    break;
                default:
                    payments = payments.OrderBy(a => a.Id).ToList();
                    break;
            }

            //Pagination
            const int PageSize = 5;
            int TotalRecords = payments.Count;
            int NumPages = (int)Math.Ceiling(Convert.ToDecimal(TotalRecords / (decimal)PageSize));
            ViewBag.NumPages = NumPages;
            ViewBag.CurrentPage = CurrentPage;
            ViewBag.TotalRecords = TotalRecords;
            payments = payments.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            return View(payments);
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            var order = _context.Orders.Select(a => new {
                Id = a.Id,
                FullDetails = a.Customer.Name + " " + a.RequestDate.ToString()
            }).ToList();
            ViewData["OrderId"] = new SelectList(order, "Id", "FullDetails");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cost,PendingDate,Date,OrderId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var order = _context.Orders.Select(a => new {
                Id = a.Id,
                FullDetails = a.Customer.Name + " " + a.RequestDate.ToString()
            }).ToList();
            ViewData["OrderId"] = new SelectList(order, "Id", "FullDetails",payment.OrderId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            var order = _context.Orders.Select(a => new {
                Id = a.Id,
                FullDetails = a.Customer.Name + " " + a.RequestDate.ToString()
            }).ToList();
            ViewData["OrderId"] = new SelectList(order, "Id", "FullDetails",payment.OrderId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cost,PendingDate,Date,OrderId")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
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
            var order = _context.Orders.Select(a => new {
                Id = a.Id,
                FullDetails = a.Customer.Name + " " + a.RequestDate.ToString()
            }).ToList();
            ViewData["OrderId"] = new SelectList(order, "Id", "FullDetails", payment.OrderId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Payments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Payments'  is null.");
            }
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
          return (_context.Payments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
