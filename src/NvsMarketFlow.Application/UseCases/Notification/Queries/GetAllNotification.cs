using MediatR;
using NvsMarketFlow.Application.Responses.Notification;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Notification.Queries;

public class GetAllNotification
{
    public sealed record GetAllNotificationQuery(
        Guid UserId,
        bool? Read,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetNotificationResponse>>;

    public class GetAllNotificationQueryHandler : IRequestHandler<GetAllNotificationQuery, PagedResult<GetNotificationResponse>>
    {
        private readonly INotificationReadOnlyRepository _notificationReadOnlyRepository;

        public GetAllNotificationQueryHandler(INotificationReadOnlyRepository notificationReadOnlyRepository)
        {
            _notificationReadOnlyRepository = notificationReadOnlyRepository;
        }

        public async Task<PagedResult<GetNotificationResponse>> Handle(GetAllNotificationQuery request, CancellationToken ct)
        {
            var result = await _notificationReadOnlyRepository.GetAllAsync(
                request.UserId, request.Read, request.Page, request.PageSize, ct);

            var items = result.Items
                .Select(n => new GetNotificationResponse
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    Read = n.Read,
                    CreatedAt = n.CreatedAt
                })
                .ToList();

            return new PagedResult<GetNotificationResponse>(
                items, result.Page, result.PageSize, result.TotalItems, result.TotalPages);
        }
    }
}