﻿using Shared.Queue.Events.Interfaces;
using Shared.Queue.Models;

namespace Shared.Queue.Events;

public class StockReservedEvent : IStockReservedEvent
{
    public StockReservedEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;
    }

    public List<OrderItemMessage> OrderItems { get; set; } = new();

    public Guid CorrelationId { get; }
}
