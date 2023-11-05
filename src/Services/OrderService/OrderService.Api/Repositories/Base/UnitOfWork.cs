using OrderService.Api.Repositories.EntityFramework.Abstract;

namespace OrderService.Api.Repositories.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly IEfOrderRepository _orderRepository;

    public UnitOfWork(
        IEfOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
}
