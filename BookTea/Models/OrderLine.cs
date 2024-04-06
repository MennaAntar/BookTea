using System.ComponentModel.DataAnnotations;

namespace BookTea.Models
{
    public class OrderLine
    {
        [Key]
        public int OL_Id { get; set; }
        public int TotalProductPrice { get; set; }
        public int ProductQuantityRequired { get; set; }

        //FK
        public int BookId { get; set; }
        public Book? Book { get; set; }
        //FK
        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
