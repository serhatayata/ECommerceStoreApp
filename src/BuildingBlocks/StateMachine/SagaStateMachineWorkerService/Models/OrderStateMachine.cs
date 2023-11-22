using MassTransit;
using Shared.Queue.Events.Interfaces;

namespace SagaStateMachineWorkerService.Models;

public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
    public State OrderCreated { get; private set; }

    public OrderStateMachine()
    {
        InstanceState(o => o.CurrentState);

        // If same order Id not exists , CorrelateBy<int> compare by OrderStateInstance id with event message order Id
        // If exists, not create a new one,
        // If not exists, create a new one by using SelectId and use Guid for CorrelationId
        Event(() => OrderCreatedRequestEvent,
              y => y.CorrelateBy<int>(d => d.OrderId, 
                                      z => z.Message.OrderId)
                                     .SelectId(context => Guid.NewGuid()));

        // During the first state, if OrderCreatedRequestEvent comes 
        Initially(
            //Initial to OrderCreated
            When(OrderCreatedRequestEvent)
            .Then(context =>
            {
                // context.Instace will be added to DB, context.Data is from message queue data
                context.Saga.BuyerId = context.Message.BuyerId;
                context.Saga.OrderId = context.Message.OrderId;
                context.Saga.CreatedDate = DateTime.Now;
                context.Saga.CardName = context.Message.Payment.CardName;
                context.Saga.CardNumber = context.Message.Payment.CardNumber;
                context.Saga.CVV = context.Message.Payment.CVV;
                context.Saga.Expiration = context.Message.Payment.Expiration;
                context.Saga.TotalPrice = context.Message.Payment.TotalPrice;
            })
            .Then(context =>
            {
                Console.WriteLine($"{OrderCreatedRequestEvent} before : {context.Saga}");
            })
            .TransitionTo(OrderCreated)
            .Then(context =>
            {
                Console.WriteLine($"{OrderCreatedRequestEvent} after : {context.Saga}");
            })
        );

    }
}
