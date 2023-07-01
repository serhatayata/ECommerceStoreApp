using EventBus.Base.Events;

namespace BasketService.Api.IntegrationEvents.Events
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; init; }

        public OrderStartedIntegrationEvent(string userId)
            => UserId = userId;
    }
}
