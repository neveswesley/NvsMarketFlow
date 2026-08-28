using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Notification.Commands;

public class MarkNotificationAsRead
{
    public sealed record MarkNotificationAsReadCommand(Guid Id) : IRequest;

    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand>
    {
        private readonly INotificationReadOnlyRepository _notificationReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotificationAsReadCommandHandler(
            INotificationReadOnlyRepository notificationReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationReadOnlyRepository = notificationReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarkNotificationAsReadCommand command, CancellationToken ct)
        {
            var notification = await _notificationReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (notification is null)
                throw new NotFoundException($"Notification with id '{command.Id}' not found.");

            notification.MarkAsRead();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}