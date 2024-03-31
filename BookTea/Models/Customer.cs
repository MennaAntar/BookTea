namespace BookTea.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set;}
        public string? Country { get; set; }

        public List<Order>? Orders { get; set; }
    }
}
