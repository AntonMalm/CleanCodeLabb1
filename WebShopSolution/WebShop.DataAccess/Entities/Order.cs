namespace WebShop.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; } // Changed to OrderItem
        public double TotalPrice { get; set; }
    }
}

