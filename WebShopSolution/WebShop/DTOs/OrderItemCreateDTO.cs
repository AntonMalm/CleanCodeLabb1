namespace WebShop.DTOs;

public class OrderItemCreateDTO
{
    public int ProductId { get; set; }  // Only ProductId, not full Product object
    public int Quantity { get; set; }
    public double Price { get; set; }
}