namespace BookTea.Models
{
    public class OrderLine
    {
        public int OL_Id { get; set; }
        public int TotalProductPrice { get; set; }
        public int ProductQuantityRequired { get; set; }

        //FK
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        //FK
        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
