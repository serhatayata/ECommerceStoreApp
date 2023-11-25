using MassTransit;
using SagaStateMachineWorkerService.Extensions;
using Shared.Queue.Events;
using Shared.Queue.Events.Interfaces;
using Shared.Queue.Messages;
using Shared.Queue.Models;

namespace SagaStateMachineWorkerService.Models;

public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
    public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
    public Event<IStockReservedEvent> StockReservedEvent { get; set; }
    public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }
    public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
    public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }

    public State OrderCreated { get; private set; }
    public State StockReserved { get; private set; }
    public State StockNotReserved { get; private set; }
    public State PaymentCompleted { get; private set; }
    public State PaymentFailed { get; private set; }

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

        // When stock reserved event is consumed, which correlation Id row we are going to change.
        Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        // When stock NOT reserved event is consumed, which correlation Id row we are going to change.
        Event(() => StockNotReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        // When payment completed event is consumed, which correlation Id row we are going to change.
        Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));



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

        // During orderCreated state, if StockReservedEvent is consumed, change state to StockReserved
        // During orderCreated state, if StockNotReservedEvent is consumed, change state to StockNotReserved
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
                    },
                    BuyerId = context.Saga.BuyerId
                })
            .Then(context =>
            {
                Console.WriteLine($"{StockReservedEvent} after : {context.Saga}");
            }),
            When(StockNotReservedEvent)
            .TransitionTo(StockNotReserved)
            .Publish(context => new OrderFailedRequestEvent()
            {
                OrderId = context.Saga.OrderId,
                Reason = context.Message.Reason
            })
            .Then(context =>
            {
                Console.WriteLine($"{StockNotReservedEvent} after : {context.Saga}");
            })
        );

        // During stockReserved status, if payment completed event is consumed, status will be changed to PaymentCompleted
        // Then we publish an event for order service to change the order's status to complete
        // During stockReserved status, if payment NOT completed event is consumed, status will be changed to PaymentNOTCompleted
        // Then we publish an event for order service to change the order's status to fail and
        // send message for stock service to change the stock of the products
        During(StockReserved,
            When(PaymentCompletedEvent)
            .TransitionTo(PaymentCompleted)
            .Publish(context => new OrderCompletedRequestEvent()
            {
                OrderId = context.Saga.OrderId
            })
            .Then(context =>
            {
                Console.WriteLine($"{PaymentCompletedEvent} after : {context.Saga}");
            })
            .Finalize(),
            When(PaymentFailedEvent)
            .Publish(context => new OrderFailedRequestEvent()
            {
                OrderId = context.Saga.OrderId,
                Reason = context.Message.Reason
            })
            .Send(new Uri($"queue:{MessageBrokerExtensions.GetQueueName<StockRollbackMessage>()}"),
                  context => new StockRollbackMessage()
                  {
                      OrderItems = context.Message.OrderItems
                  })
            .TransitionTo(PaymentFailed)
            .Then(context =>
            {
                Console.WriteLine($"{PaymentFailedEvent} after : {context.Saga}");
            })
        );

        // This can be used to delete if the state of the record is Finalize
        //SetCompletedWhenFinalized();
    }
}
