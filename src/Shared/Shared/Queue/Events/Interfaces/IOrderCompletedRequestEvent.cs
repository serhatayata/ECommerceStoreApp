using MassTransit;

namespace Shared.Queue.Events.Interfaces;

/// <summary>
/// Normally we use CorrelateBy here for request events from state machine 
/// but this is the last event for process, so we don't need to use it.
/// </summary>
public interface IOrderCompletedRequestEvent
{
    int OrderId { get; set; }
}
