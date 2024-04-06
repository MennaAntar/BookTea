using BookTea.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookTea.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>().HasKey(tbl => new { tbl.BookId, tbl.AuthorId });
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }
        public DbSet<CostSpecification> CostsSpecifications { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PublishingHouse> PublishingHouses { get; set; }
        public DbSet<ShippingCompany> ShippingCompanies { get; set; }
    }
}
