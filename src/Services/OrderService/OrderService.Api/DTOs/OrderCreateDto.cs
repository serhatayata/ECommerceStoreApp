namespace OrderService.Api.DTOs;

public class OrderCreateDto
{
    public string BuyerId { get; set; }

    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    public PaymentDto Payment { get; set; }
    public AddressDto Address { get; set; }
}
