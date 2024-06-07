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
        public async Task<IActionResult> Index(string term, string orderby, int CurrentPage = 1)
        {
            //Search
            var bookAuthors = await _context.BooksAuthors.Include(c=>c.Author).Include(c=>c.Book).ToListAsync();
            if (!String.IsNullOrEmpty(term))
            {
                bookAuthors = bookAuthors.Where(a => a.Author.FirstName.ToString().Contains(term) || a.Book.Title.Contains(term)).ToList();

            }

            //Sort
            ViewBag.OrderAuthor = orderby == "Author" ? "Author_des" : "Author";
            ViewBag.OrderBook = orderby == "Book" ? "Book_des" : "Book";
            switch (orderby)
            {
                case "Author":
                    bookAuthors = bookAuthors.OrderBy(a => a.Author.FirstName).ToList();
                    break;
                case "Author_des":
                    bookAuthors = bookAuthors.OrderByDescending(a => a.Author.FirstName).ToList();
                    break;
                case "Book":
                    bookAuthors = bookAuthors.OrderBy(a => a.Book.Title).ToList();
                    break;
                case "Book_des":
                    bookAuthors = bookAuthors.OrderByDescending(a => a.Book.Title).ToList();
                    break;
            }

            //Pagination
            const int PageSize = 5;
            int TotalRecords = bookAuthors.Count;
            int NumPages = (int)Math.Ceiling(Convert.ToDecimal(TotalRecords / (decimal)PageSize));
            ViewBag.NumPages = NumPages;
            ViewBag.CurrentPage = CurrentPage;
            ViewBag.TotalRecords = TotalRecords;
            bookAuthors = bookAuthors.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

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
            //To get the full name of author
            var authors = _context.Authors.Select(a => new{
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName
            }).ToList();

            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "Title");
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

            //To get the full name of author
            var authors = _context.Authors.Select(a => new {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName
            }).ToList();

            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName", bookAuthor.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "ISBN", "Title", bookAuthor.BookId);
            return View(bookAuthor);
        }

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
