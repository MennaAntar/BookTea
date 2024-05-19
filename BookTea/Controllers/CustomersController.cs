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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string term, string orderby = "Id")
        {
            var customer = await _context.Customers.ToListAsync();
            if (!String.IsNullOrEmpty(term))
            {
                customer = customer.Where(a => a.Name.Contains(term) ||
                a.Email.Contains(term) || a.DateOfBirth.ToString().Contains(term) || a.Address.Contains(term) ||
                a.Country.Contains(term)).ToList();

            }

            //Sort
            ViewBag.OrderName = orderby == "Name" ? "Name_des" : "Name";
            ViewBag.OrderEmail = orderby == "Email" ? "Email_des" : "Email";
            ViewBag.OrderDateOfBirth = orderby == "DateOfBirth" ? "DateOfBirth_des" : "DateOfBirth";
            ViewBag.OrderAddress = orderby == "Address" ? "Address_des" : "Address";
            ViewBag.OrderCountry = orderby == "Country" ? "Country_des" : "Country";
            switch (orderby)
            {
                case "Name":
                    customer = customer.OrderBy(a => a.Name).ToList();
                    break;
                case "Name_des":
                    customer = customer.OrderByDescending(a => a.Name).ToList();
                    break;
                case "Email":
                    customer = customer.OrderBy(a => a.Email).ToList();
                    break;
                case "Email_des":
                    customer = customer.OrderByDescending(a => a.Email).ToList();
                    break;
                case "DateOfBirth":
                    customer = customer.OrderBy(a => a.DateOfBirth).ToList();
                    break;
                case "DateOfBirth_des":
                    customer = customer.OrderByDescending(a => a.DateOfBirth).ToList();
                    break;
                case "Address":
                    customer = customer.OrderBy(a => a.Address).ToList();
                    break;
                case "Address_des":
                    customer = customer.OrderByDescending(a => a.Address).ToList();
                    break;
                case "Country":
                    customer = customer.OrderBy(a => a.Country).ToList();
                    break;
                case "Country_des":
                    customer = customer.OrderByDescending(a => a.Country).ToList();
                    break;
                default:
                    customer = customer.OrderBy(a => a.Id).ToList();
                    break;
            }

            return View(customer);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Password,Name,Email,DateOfBirth,Address,Country")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Password,Name,Email,DateOfBirth,Address,Country")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
