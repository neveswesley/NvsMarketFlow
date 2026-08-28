using MediatR;
using NvsMarketFlow.Application.Events;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.EventHandlers;

public class ProductMaximumStockExceededEventHandler : INotificationHandler<ProductMaximumStockExceededEvent>
{
    private readonly INotificationWriteOnlyRepository _notificationWriteOnlyRepository;

    public ProductMaximumStockExceededEventHandler(INotificationWriteOnlyRepository notificationWriteOnlyRepository)
    {
        _notificationWriteOnlyRepository = notificationWriteOnlyRepository;
    }

    public async Task Handle(ProductMaximumStockExceededEvent notification, CancellationToken ct)
    {
        var entity = new Domain.Entities.Notification(
            notification.UserId,
            "Estoque acima do máximo",
            $"O produto '{notification.ProductName}' ultrapassou o estoque máximo ({notification.CurrentStock} / máximo {notification.MaximumStock}).");

        await _notificationWriteOnlyRepository.CreateAsync(entity, ct);
    }
}