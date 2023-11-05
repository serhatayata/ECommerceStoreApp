using OrderService.Api.Entities;
using OrderService.Api.Models.Base;
using OrderService.Api.Repositories.EntityFramework.Abstract;
using OrderService.Api.Utilities.Results;

namespace OrderService.Api.Repositories.EntityFramework.Concrete;

public class EfOrderRepository : IEfOrderRepository
{
    public Task<Result> AddAsync(Order entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(StringModel model)
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<IReadOnlyList<Order>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DataResult<Order>> GetAsync(StringModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Order entity)
    {
        throw new NotImplementedException();
    }
}
