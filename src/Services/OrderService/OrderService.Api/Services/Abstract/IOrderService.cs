using OrderService.Api.Models.Base;
using OrderService.Api.Models.Enums;
using OrderService.Api.Models.OrderModels;
using OrderService.Api.Utilities.Results;

namespace OrderService.Api.Services.Abstract;

public interface IOrderService
{
    Task<DataResult<int>> AddAsync(OrderAddModel model);
    Task<Result> UpdateAsync(OrderUpdateModel model);
    Task<Result> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
    Task<Result> DeleteAsync(int model);

    Task<DataResult<OrderModel>> GetAsync(int model);
    Task<DataResult<IReadOnlyList<OrderModel>>> GetAllAsync();
}
