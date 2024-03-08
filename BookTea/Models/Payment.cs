namespace BookTea.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public DateTime PendingDate { get; set; }
        public DateTime Date { get; set; }

        //FK
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        //FK
        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
    }
}
