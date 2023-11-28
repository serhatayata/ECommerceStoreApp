using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaStateMachineWorkerService.Infrastructure.StateMappings;

namespace SagaStateMachineWorkerService.Infrastructure.Contexts;

public class OrderStateDbContext : SagaDbContext
{
    public OrderStateDbContext(DbContextOptions<OrderStateDbContext> options) : base(options)
    {
        
    }

    protected override IEnumerable<ISagaClassMap> Configurations 
    { 
        get { yield return new OrderStateMap(); }
    }
}
