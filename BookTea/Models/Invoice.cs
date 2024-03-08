namespace BookTea.Models
{
    public class Invoice
    {
        public int InvId { get; set; }
        
        //FK
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
