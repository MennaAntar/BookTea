namespace BookTea.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? Description { get; set; }
        public int Ranking { get; set; }
        public string? PhotoUrl { get; set; }

        public List<OrderLine>? OrderLines { get; set; }

        //FK
        public int ProducerId { get; set; }
        public Producer? Producer { get; set; } 
    }
}
