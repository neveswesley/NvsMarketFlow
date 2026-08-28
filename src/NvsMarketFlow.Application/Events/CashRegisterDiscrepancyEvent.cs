using MediatR;

namespace NvsMarketFlow.Application.Events;

public sealed record CashRegisterDiscrepancyEvent(
    Guid CashRegisterId,
    Guid UserId,
    decimal Discrepancy
) : INotification;