namespace BookTea.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public DateTime RequestDate { get; set; }

        public List<OrderLine>? OrderLines { get; set; }

        //FK
        public int ShippingCompanyId { get; set; }
        public ShippingCompany? ShippingCompany { get; set; }
        //FK
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
