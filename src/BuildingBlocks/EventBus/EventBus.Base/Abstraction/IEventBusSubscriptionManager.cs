using EventBus.Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Base.Abstraction
{
    public interface IEventBusSubscriptionManager
    {
        /// <summary>
        /// Whether there is an event or not
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// This event will be removed when removed
        /// </summary>
        event EventHandler<string> OnEventRemoved;
        /// <summary>
        /// Add subscription
        /// </summary>
        /// <typeparam name="T">Event</typeparam>
        /// <typeparam name="TH">Handler</typeparam>
        void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
        /// <summary>
        /// Remove subscription
        /// </summary>
        /// <typeparam name="T">Event</typeparam>
        /// <typeparam name="TH">Handler</typeparam>
        void RemoveSubscription<T, TH>() where TH : IIntegrationEventHandler<T> where T : IntegrationEvent;
        /// <summary>
        /// Whether the T event is subscribed or not
        /// </summary>
        /// <typeparam name="T">Event</typeparam>
        /// <returns>whether subscibed to any or not</returns>
        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        /// <summary>
        /// Whether the event which comes with <paramref name="eventName"/> is subscribed or not
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns>whether subscribed to any or not</returns>
        bool HasSubscriptionsForEvent(string eventName);
        /// <summary>
        /// Get type of the event which can be found with <paramref name="eventName"/>
        /// </summary>
        /// <param name="eventName">event name</param>
        /// <returns>type of event</returns>
        Type GetEventTypeByName(string eventName);
        /// <summary>
        /// Clear all subscriptions
        /// </summary>
        void Clear();
        /// <summary>
        /// For T event, get all subscriptions
        /// </summary>
        /// <typeparam name="T">Event</typeparam>
        /// <returns></returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;
        /// <summary>
        /// For <paramref name="eventName"/> , get all subscription
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        /// <summary>
        /// Key of integration event
        /// </summary>
        /// <typeparam name="T">Event</typeparam>
        /// <returns></returns>
        string GetEventKey<T>();
    }
}
