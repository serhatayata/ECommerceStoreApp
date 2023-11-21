using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaStateMachineWorkerService.Models;

namespace SagaStateMachineWorkerService.Infrastructure.StateMappings;

public class OrderStateMap : SagaClassMap<OrderStateInstance>
{
    protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
    {
        entity.Property(o => o.BuyerId).HasMaxLength(256);
        entity.Property(o => o.BuyerId).IsRequired(true);

        entity.Property(o => o.TotalPrice).HasPrecision(10, 2);

        entity.Property(o => o.OrderId).IsRequired(true);
    }
}
