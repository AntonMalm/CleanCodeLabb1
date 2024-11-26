namespace WebShop.DTOs;

public class OrderCreateDTO
{
    public int CustomerId { get; set; }
    public List<OrderItemCreateDTO> OrderItems { get; set; }
    public double TotalPrice { get; set; }
}