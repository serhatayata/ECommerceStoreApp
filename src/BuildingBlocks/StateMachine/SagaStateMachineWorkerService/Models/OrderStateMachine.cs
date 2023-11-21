using MassTransit;

namespace SagaStateMachineWorkerService.Models;

public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
{
}
