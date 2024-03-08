namespace BookTea.Models
{
    public class Producer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }

        public List<Product>? Products { get; set; }
    }
}
