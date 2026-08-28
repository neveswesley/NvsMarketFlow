using MediatR;
using NvsMarketFlow.Application.Events;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.EventHandlers;

public class ProductLowStockEventHandler : INotificationHandler<ProductLowStockEvent>
{
    private readonly INotificationWriteOnlyRepository _notificationWriteOnlyRepository;

    public ProductLowStockEventHandler(INotificationWriteOnlyRepository notificationWriteOnlyRepository)
    {
        _notificationWriteOnlyRepository = notificationWriteOnlyRepository;
    }

    public async Task Handle(ProductLowStockEvent notification, CancellationToken ct)
    {
        var entity = new Domain.Entities.Notification(
            notification.UserId,
            "Estoque baixo",
            $"O produto '{notification.ProductName}' está com estoque baixo ({notification.CurrentStock} / mínimo {notification.MinimumStock}).");

        await _notificationWriteOnlyRepository.CreateAsync(entity, ct);
    }
}