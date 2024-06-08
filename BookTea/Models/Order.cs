namespace BookTea.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double TotalCost { get; set; }
        public DateTime RequestDate { get; set; }

        //FK
        public int ShippingCompanyId { get; set; }
        public ShippingCompany? ShippingCompany { get; set; }
        //FK
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public Payment? Payment { get; set; }
        public List<OrderLine>? OrderLines { get; set; }
    }
}
