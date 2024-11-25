namespace WebShop.DTOs;

public class OrderCreateDTO
{
    public int CustomerId { get; set; } // Only CustomerId, not full Customer object
    public List<OrderItemCreateDTO> OrderItems { get; set; }  // Only ProductId and other details
    public double TotalPrice { get; set; }
}