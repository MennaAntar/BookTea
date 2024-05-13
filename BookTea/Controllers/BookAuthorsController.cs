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
    public class BookAuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookAuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookAuthors
        public async Task<IActionResult> Index(string term)
        {
            //if (!string.IsNullOrEmpty(term))
            //    ViewBag.term = term;

            var bookAuthors = await _context.BooksAuthors.Include(c=>c.Author).Include(c=>c.Book).ToListAsync();
            if (!String.IsNullOrEmpty(term))
            {
                bookAuthors = bookAuthors.Where(a => a.Author.FirstName.ToString().Contains(term) || a.Book.Title.Contains(term)).ToList();

            }
            return View(bookAuthors);
        }

        // GET: BookAuthors/Details/5
        public async Task<IActionResult> Details(int? BookId, int? AuthorId)
        {
            if (BookId == null|| AuthorId==null || _context.BooksAuthors == null)
            {
                return NotFound();
            }

            var bookAuthor = await _context.BooksAuthors
                .Include(b => b.Author)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.BookId == BookId&&m.AuthorId==AuthorId);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            return View(bookAuthor);
        }

        // GET: BookAuthors/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id");
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "ISBN");
            return View();
        }

        // POST: BookAuthors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,AuthorId")] BookAuthor bookAuthor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookAuthor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", bookAuthor.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "ISBN", bookAuthor.BookId);
            return View(bookAuthor);
        }

        //// GET: BookAuthors/Edit/5
        //public async Task<IActionResult> Edit(int? BookId, int? AuthorId)
        //{
        //    if (BookId==null|| AuthorId==null || _context.BooksAuthors == null)
        //    {
        //        return NotFound();
        //    }

        //    var bookAuthor = await _context.BooksAuthors
        //        .Include(b => b.Author)
        //        .Include(b => b.Book)
        //        .FirstOrDefaultAsync(m => m.BookId == BookId && m.AuthorId == AuthorId);

        //    if (bookAuthor == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", bookAuthor.AuthorId);
        //    ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "ISBN", bookAuthor.BookId);
        //    return View(bookAuthor);
        //}

        //// POST: BookAuthors/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int? BookId, int? AuthorId, [Bind("BookId,AuthorId")] BookAuthor bookAuthor)
        //{
        //    if (BookId != bookAuthor.BookId||AuthorId!=bookAuthor.AuthorId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(bookAuthor);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BookAuthorExists(bookAuthor.BookId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", bookAuthor.AuthorId);
        //    ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "ISBN", bookAuthor.BookId);
        //    return View(bookAuthor);
        //}

        // GET: BookAuthors/Delete/5
        public async Task<IActionResult> Delete(int? BookId, int? AuthorId)
        {
            if (BookId==null||AuthorId==null || _context.BooksAuthors == null)
            {
                return NotFound();
            }

            var bookAuthor = await _context.BooksAuthors
                .Include(b => b.Author)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.BookId == BookId && m.AuthorId == AuthorId);
            if (bookAuthor == null)
            {
                return NotFound();
            }

            return View(bookAuthor);
        }

        // POST: BookAuthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? BookId, int? AuthorId)
        {
            if (_context.BooksAuthors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BooksAuthors'  is null.");
            }
            var bookAuthor = await _context.BooksAuthors.FirstOrDefaultAsync(m => m.AuthorId == AuthorId && m.BookId == BookId);
            if (bookAuthor != null)
            {
                _context.BooksAuthors.Remove(bookAuthor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookAuthorExists(int id)
        {
          return (_context.BooksAuthors?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
