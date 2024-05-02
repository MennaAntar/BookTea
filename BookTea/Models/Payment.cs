namespace BookTea.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public DateTime PendingDate { get; set; }
        public DateTime Date { get; set; }

        //FK
        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
