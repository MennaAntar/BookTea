namespace BookTea.Models
{
    public class ShippingCompany
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public DateTime Date { get; set; }
        public int Weight { get; set; }
        public string? Destination { get; set; }

        public List<CostSpecification>? CostSpecifications { get; set; }

        public List<Order>? Orders { get; set; }
    }
}
