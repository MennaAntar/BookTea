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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string term , string orderby= "TotalCost", int CurrentPage = 1)
        {
            if (!string.IsNullOrEmpty(term))
                ViewBag.term = term;

            ViewBag.orderby = orderby;
            if (ViewBag.orderby != null)
                orderby = ViewBag.orderby;
            ViewBag.orderCost = orderby == "TotalCost" ? "TotalCost_des" : "TotalCost";
            //Search
            var order = await _context.Orders.Include(c=>c.Customer).Include(c => c.ShippingCompany).ToListAsync();
            if (!String.IsNullOrEmpty(term))
            {
                order = order.Where(a => a.Id.ToString().Contains(term) || a.TotalCost.ToString().Contains(term) || a.ShippingCompany.Destination.ToString().Contains(term) || a.RequestDate.ToString().Contains(term) || a.Customer.Name.ToString().Contains(term)).ToList();

            }

            //sort
            switch (orderby)
            {
                case "TotalCost":
                    order = order.OrderBy(orl => orl.TotalCost).ToList();
                    break;
                case "TotalCost_des":
                    order = order.OrderByDescending(orl => orl.TotalCost).ToList();
                    break;
            }

            //Pagination

            const int PageSize = 5;
            int TotalRecords = order.Count;
            int NumPages = (int)Math.Ceiling(Convert.ToDecimal(TotalRecords / (decimal)PageSize));
            ViewBag.NumPages = NumPages;
            ViewBag.CurrentPage = CurrentPage;
            ViewBag.TotalRecords = TotalRecords;
            order = order.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            return View(order);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.ShippingCompany)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");

            var SC = _context.ShippingCompanies.Select(a => new {
                Id = a.Id,
                FullDetails = a.Destination +" "+a.Weight.ToString()
            }).ToList();
            ViewData["ShippingCompanyId"] = new SelectList(SC, "Id", "FullDetails");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RequestDate,ShippingCompanyId,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.TotalCost = 0;
                order.RequestDate = DateTime.Now;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            var SC = _context.ShippingCompanies.Select(a => new {
                Id = a.Id,
                FullDetails = a.Destination +" "+a.Weight.ToString()
            }).ToList();
            ViewData["ShippingCompanyId"] = new SelectList(SC, "Id", "FullDetails");
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            var SC = _context.ShippingCompanies.Select(a => new {
                Id = a.Id,
                FullDetails = a.Destination + " " + a.Weight.ToString()
            }).ToList();
            ViewData["ShippingCompanyId"] = new SelectList(SC, "Id", "FullDetails");
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TotalCost,RequestDate,ShippingCompanyId,CustomerId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", order.CustomerId);
            var SC = _context.ShippingCompanies.Select(a => new {
                Id = a.Id,
                FullDetails = a.Destination + " " + a.Weight.ToString()
            }).ToList();
            ViewData["ShippingCompanyId"] = new SelectList(SC, "Id", "FullDetails");
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.ShippingCompany)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //---------------------Order line-------------------------
        public IActionResult MoreDetails(int id)
        {
            var order = _context.Orders.Where(x => x.Id == id).Include(o => o.Customer).Include(o => o.OrderLines).ThenInclude(ol => ol.Book).FirstOrDefault();
            
            return View(order);
        }
        
        public IActionResult CreateOrderLine(int orderId)
        {
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "Title");
            var model = new OrderLine { OrderId = orderId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrderLine([Bind("OL_Id,ProductQuantityRequired,Price,BookId,OrderId")] OrderLine orderLine)
        {
            if (ModelState.IsValid)
            {
                var Book = _context.Books.Where(b => b.ISBN == orderLine.BookId).FirstOrDefault();
                var order = _context.Orders.Where(o => o.Id == orderLine.OrderId).FirstOrDefault();
                if (orderLine.ProductQuantityRequired <= Book?.Quantity)
                {
                    orderLine.Price = orderLine.ProductQuantityRequired * Book.Price;
                    Book.Quantity-=orderLine.ProductQuantityRequired;
                    order.TotalCost += orderLine.Price;
                    _context.Update(Book);
                    _context.Update(order);
                    _context.Add(orderLine);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MoreDetails), new { id = order.Id });
                }
            }
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "Title", orderLine.BookId);
            ViewData["OrderId"] = orderLine.OrderId;
            return View(orderLine);
        }

        public async Task<IActionResult> DeleteOrderLine(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrderLine(int id)
        {
            if (_context.OrderLines == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OrderLines'  is null.");
            }
            var orderLine = await _context.OrderLines.FindAsync(id);
            if (orderLine != null)
            {

                var Book = _context.Books.Where(b => b.ISBN == orderLine.BookId).FirstOrDefault();
                var order = _context.Orders.Where(o => o.Id == orderLine.OrderId).FirstOrDefault();
                Book.Quantity += orderLine.ProductQuantityRequired;
                order.TotalCost -= orderLine.Price;
                _context.Update(Book);
                _context.Update(order);
                _context.OrderLines.Remove(orderLine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MoreDetails), new { id = orderLine.OrderId });

            }

            return NotFound();
        }
    }
}
