using MediatR;

namespace NvsMarketFlow.Application.Events;

public sealed record ProductLowStockEvent(
    Guid ProductId,
    string ProductName,
    decimal CurrentStock,
    decimal MinimumStock,
    Guid UserId
) : INotification; 