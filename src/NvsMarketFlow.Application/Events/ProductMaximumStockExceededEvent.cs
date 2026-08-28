using MediatR;

namespace NvsMarketFlow.Application.Events;

public sealed record ProductMaximumStockExceededEvent(
    Guid ProductId,
    string ProductName,
    decimal CurrentStock,
    decimal MaximumStock,
    Guid UserId
) : INotification;