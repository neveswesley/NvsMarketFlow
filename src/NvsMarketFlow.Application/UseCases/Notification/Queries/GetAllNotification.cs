using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.Notification;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Notification.Queries;

public class GetAllNotification
{
    public sealed record GetAllNotificationQuery(
        bool? Read,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetNotificationResponse>>;

    public class GetAllNotificationQueryHandler : IRequestHandler<GetAllNotificationQuery, PagedResult<GetNotificationResponse>>
    {
        private readonly INotificationReadOnlyRepository _notificationReadOnlyRepository;
        private readonly ICurrentUserContext _currentUserContext;


        public GetAllNotificationQueryHandler(INotificationReadOnlyRepository notificationReadOnlyRepository, ICurrentUserContext currentUserContext)
        {
            _notificationReadOnlyRepository = notificationReadOnlyRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<PagedResult<GetNotificationResponse>> Handle(GetAllNotificationQuery request, CancellationToken ct)
        {
            var userId = _currentUserContext.UserId
                          ?? throw new UnauthorizedException("User not authenticated.");

            var result = await _notificationReadOnlyRepository.GetAllAsync(
                userId, request.Read, request.Page, request.PageSize, ct);

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