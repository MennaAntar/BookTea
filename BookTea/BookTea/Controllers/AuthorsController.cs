﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookTea.Context;
using BookTea.Models;
using Microsoft.Extensions.Hosting;

namespace BookTea.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHost;

        public AuthorsController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        // GET: Authors
        public async Task<IActionResult> Index(string term, string orderby = "Id", int CurrentPage = 1)
        {
            //Search
            var author = await _context.Authors.ToListAsync();
            if (!String.IsNullOrEmpty(term))
            {
                author = author.Where(a => a.Id.ToString().Contains(term)|| a.DateOfBirth.ToString().Contains(term)
                || a.DateOfDeath.ToString().Contains(term) || a.Nationality.Contains(term) || a.LastName.Contains(term)
                || a.FirstName.Contains(term)).ToList();

            }

            //Sort
            ViewBag.OrderFirstName = orderby == "FirstName" ? "FirstName_des" : "FirstName";
            ViewBag.OrderLastName = orderby == "LastName" ? "LastName_des" : "LastName";
            ViewBag.OrderDateOfBirth = orderby == "DateOfBirth" ? "DateOfBirth_des" : "DateOfBirth";
            ViewBag.OrderDateOfDeath = orderby == "DateOfDeath" ? "DateOfDeath_des" : "DateOfDeath";
            ViewBag.OrderNationality = orderby == "Nationality" ? "Nationality_des" : "Nationality";
            switch (orderby)
            {
                case "FirstName":
                    author = author.OrderBy(a => a.FirstName).ToList();
                    break;
                case "FirstName_des":
                    author = author.OrderByDescending(a => a.FirstName).ToList();
                    break;
                case "LastName":
                    author = author.OrderBy(a => a.LastName).ToList();
                    break;
                case "LastName_des":
                    author = author.OrderByDescending(a => a.LastName).ToList();
                    break;
                case "DateOfBirth":
                    author = author.OrderBy(a => a.DateOfBirth).ToList();
                    break;
                case "DateOfBirth_des":
                    author = author.OrderByDescending(a => a.DateOfBirth).ToList();
                    break;
                case "DateOfDeath":
                    author = author.OrderBy(a => a.DateOfDeath).ToList();
                    break;
                case "DateOfDeath_des":
                    author = author.OrderByDescending(a => a.DateOfDeath).ToList();
                    break;
                case "Nationality":
                    author = author.OrderBy(a => a.Nationality).ToList();
                    break;
                case "Nationality_des":
                    author = author.OrderByDescending(a => a.Nationality).ToList();
                    break;
                default:
                    author = author.OrderBy(a => a.Id).ToList();
                    break;
            }

            //Pagination
            const int PageSize = 5;
            int TotalRecords = author.Count;
            int NumPages = (int)Math.Ceiling(Convert.ToDecimal(TotalRecords / (decimal)PageSize));
            ViewBag.NumPages = NumPages;
            ViewBag.CurrentPage = CurrentPage;
            ViewBag.TotalRecords = TotalRecords;
            author = author.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            return View(author);
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DateOfBirth,DateOfDeath,Nationality,ImageFile")] Author author)
        {
            if (ModelState.IsValid)
            {
                author.PhotoUrl = UploadImage(author);
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth,DateOfDeath,Nationality,ImageFile")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (author.ImageFile != null)
                    {
                        string uniqueFileName = UploadImage(author);
                        author.PhotoUrl = uniqueFileName;
                    }
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Authors'  is null.");
            }
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string UploadImage(Author author)
        {
            string uniqueFileName = null;
            if (author.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images"); //بيجيب فولدر الصور اللي هتنزل عليه
                uniqueFileName = Guid.NewGuid().ToString() + "_" + author.ImageFile.FileName; //اسم الملف الاصلي
                string filePath = Path.Combine(uploadsFolder, uniqueFileName); //لما بيدمج الاتنين بيكون دا مكان الصورة الجديد
                using (var fileStream = new FileStream(filePath, FileMode.Create)) //بينقل الصورة للمكان الجديد
                {
                    author.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
