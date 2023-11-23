using MassTransit;
using SagaStateMachineWorkerService.Extensions;
using Shared.Queue.Events;
using Shared.Queue.Events.Interfaces;
using Shared.Queue.Models;

namespace SagaStateMachineWorkerService.Models;

public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
    public Event<IStockReservedEvent> StockReservedEvent { get; set; }

    public State OrderCreated { get; private set; }
    public State StockReserved { get; private set; }

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
            .Publish(context => new OrderCreatedEvent(context.Saga.CorrelationId)
            {
                OrderItems = context.Message.OrderItems
            })
            .TransitionTo(OrderCreated)
            .Then(context =>
            {
                Console.WriteLine($"{OrderCreatedRequestEvent} after : {context.Saga}");
            })
        );

        // During orderCreated state, if StockReservedEvent comes change state to StockReserved
        // After that we use send (We use send because there is only one endpoint using this)
        During(OrderCreated,
            When(StockReservedEvent)
            .TransitionTo(StockReserved)
            .Send(new Uri($"queue:{MessageBrokerExtensions.GetQueueName<StockReservedRequestPaymentEvent>()}"),
                context => new StockReservedRequestPaymentEvent(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems,
                    Payment = new PaymentMessage()
                    {
                        CardName = context.Saga.CardName,
                        CardNumber = context.Saga.CardNumber,
                        CVV = context.Saga.CVV,
                        Expiration = context.Saga.Expiration,
                        TotalPrice = context.Saga.TotalPrice
                    }
                })
            .Then(context =>
            {
                Console.WriteLine($"{StockReservedEvent} after : {context.Saga}");
            })
        );
    }
}
