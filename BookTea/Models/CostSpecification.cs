namespace BookTea.Models
{
    public class CostSpecification
    {
        public int Id { get; set; }
        public string? CityName { get; set; }
        public int DeliveryCost { get; set; }

        public int ShippingCompanyId { get; set; }
        public ShippingCompany? ShippingCompany { get; set; }
    }
}