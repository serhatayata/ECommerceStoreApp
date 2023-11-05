using OrderService.Api.Entities;
using OrderService.Api.Models.Base;
using OrderService.Api.Repositories.Base;

namespace OrderService.Api.Repositories.EntityFramework.Abstract;

public interface IEfOrderRepository : IGenericRepository<Order, StringModel>
{
}
