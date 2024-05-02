using System.ComponentModel.DataAnnotations.Schema;

namespace BookTea.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string? Nationality { get; set; }

        public string? PhotoUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public List<BookAuthor>? BookAuthors { get; set; }
    }
}
