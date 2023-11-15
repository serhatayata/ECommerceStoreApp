namespace OrderService.Api.Models.Enums;

public enum OrderStatus : byte
{
    Suspend = 1,
    Completed = 2,
    Fail = 3,
}
