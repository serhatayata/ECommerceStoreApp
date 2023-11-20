namespace OrderService.Api.DTOs;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Count { get; set; }
    public decimal Price { get; set; }
}
