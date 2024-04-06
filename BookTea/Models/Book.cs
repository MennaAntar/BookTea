using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTea.Models
{
    public class Book
    {
        [Key]
        public int ISBN { get; set; }
        public string? Title { get; set; }
        public double Price { get; set; }

        public string? PhotoUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public int Rating { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }

        public int PublishingHouseId { get; set; }
        public PublishingHouse? PublishingHouse { get; set; }

        public List<OrderLine>? OrderLines { get; set; }
        public List<BookAuthor>? BookAuthors { get; set; }
    }
}
