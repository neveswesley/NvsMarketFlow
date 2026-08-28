using MediatR;
using NvsMarketFlow.Application.Responses.AuditLog;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.AuditLog.Queries;

public class GetAllAuditLog
{
    public sealed record GetAllAuditLogQuery(
        Guid? UserId,
        string? Entity,
        string? Action,
        DateTime? StartDate,
        DateTime? EndDate,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetAuditLogResponse>>;

    public class GetAllAuditLogQueryHandler : IRequestHandler<GetAllAuditLogQuery, PagedResult<GetAuditLogResponse>>
    {
        private readonly IAuditLogReadOnlyRepository _auditLogReadOnlyRepository;

        public GetAllAuditLogQueryHandler(IAuditLogReadOnlyRepository auditLogReadOnlyRepository)
        {
            _auditLogReadOnlyRepository = auditLogReadOnlyRepository;
        }

        public async Task<PagedResult<GetAuditLogResponse>> Handle(GetAllAuditLogQuery request, CancellationToken ct)
        {
            var result = await _auditLogReadOnlyRepository.GetAllAsync(
                request.UserId,
                request.Entity,
                request.Action,
                request.StartDate,
                request.EndDate,
                request.Page,
                request.PageSize,
                ct);

            var items = result.Items
                .Select(a => new GetAuditLogResponse
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Action = a.Action,
                    Entity = a.Entity,
                    OldValue = a.OldValue,
                    NewValue = a.NewValue,
                    Date = a.Date
                })
                .ToList();

            return new PagedResult<GetAuditLogResponse>(
                items,
                result.Page,
                result.PageSize,
                result.TotalItems,
                result.TotalPages);
        }
    }
}