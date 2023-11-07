using OrderService.Api.Models.Base;
using OrderService.Api.Models.OrderModels;
using OrderService.Api.Utilities.Results;

namespace OrderService.Api.Services.Abstract;

public interface IOrderService
{
    Task<DataResult<int>> AddAsync(OrderAddModel model);
    Task<Result> UpdateAsync(OrderUpdateModel model);
    Task<Result> DeleteAsync(IntModel model);

    Task<DataResult<OrderModel>> GetAsync(IntModel model);
    Task<DataResult<IReadOnlyList<OrderModel>>> GetAllAsync();
}
