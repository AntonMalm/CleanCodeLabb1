namespace WebShop.DTOs;

public class OrderItemCreateDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}