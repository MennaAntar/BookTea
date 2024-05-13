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
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost;

        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        // GET: Books
        public async Task<IActionResult> Index(string term)
        {
            var bookAuthors = await _context.Books.Include(c => c.PublishingHouse).ToListAsync();
            if (!String.IsNullOrEmpty(term))
            {
                bookAuthors = bookAuthors.Where(a => a.Description.Contains(term) || a.Price.ToString().Contains(term)|| a.Title.Contains(term)|| a.Rating.ToString().Contains(term)|| a.Quantity.ToString().Contains(term)|| a.ISBN.ToString().Contains(term)|| a.PublishingHouseId.ToString().Contains(term)).ToList();

            }
            return View(bookAuthors);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.PublishingHouse)
                .FirstOrDefaultAsync(m => m.ISBN == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "Id", "Id");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ISBN,Title,Price,ImageFile,Rating,Description,Quantity,PublishingHouseId")] Book book)
        {
            if (ModelState.IsValid)
            {
                book.PhotoUrl = UploadImage(book);
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "Id", "Id", book.PublishingHouseId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "Id", "Id", book.PublishingHouseId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ISBN,Title,Price,ImageFile,Rating,Description,Quantity,PublishingHouseId")] Book book)
        {
            if (id != book.ISBN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (book.ImageFile != null)
                    {
                        string uniqueFileName = UploadImage(book);
                        book.PhotoUrl = uniqueFileName;
                    }
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ISBN))
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
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "Id", "Id", book.PublishingHouseId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.PublishingHouse)
                .FirstOrDefaultAsync(m => m.ISBN == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.ISBN == id)).GetValueOrDefault();
        }

        private string UploadImage(Book book)
        {
            string uniqueFileName = null;
            if (book.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images"); //بيجيب فولدر الصور اللي هتنزل عليه
                uniqueFileName = Guid.NewGuid().ToString() + "_" + book.ImageFile.FileName; //اسم الملف الاصلي
                string filePath = Path.Combine(uploadsFolder, uniqueFileName); //لما بيدمج الاتنين بيكون دا مكان الصورة الجديد
                using (var fileStream = new FileStream(filePath, FileMode.Create)) //بينقل الصورة للمكان الجديد
                {
                    book.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
