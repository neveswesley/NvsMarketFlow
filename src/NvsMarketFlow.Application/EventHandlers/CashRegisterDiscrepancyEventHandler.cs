using MediatR;
using NvsMarketFlow.Application.Events;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.EventHandlers;

public class CashRegisterDiscrepancyEventHandler : INotificationHandler<CashRegisterDiscrepancyEvent>
{
    private readonly INotificationWriteOnlyRepository _notificationWriteOnlyRepository;

    public CashRegisterDiscrepancyEventHandler(INotificationWriteOnlyRepository notificationWriteOnlyRepository)
    {
        _notificationWriteOnlyRepository = notificationWriteOnlyRepository;
    }

    public async Task Handle(CashRegisterDiscrepancyEvent notification, CancellationToken ct)
    {
        var word = notification.Discrepancy < 0 ? "faltou" : "sobrou";
        var amount = Math.Abs(notification.Discrepancy);

        var entity = new Domain.Entities.Notification(
            notification.UserId,
            "Divergência no fechamento de caixa",
            $"O fechamento do caixa apresentou divergência: {word} R$ {amount:F2}.");

        await _notificationWriteOnlyRepository.CreateAsync(entity, ct);
    }
}