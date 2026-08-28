using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface ICashRegisterReadOnlyRepository
{
    Task<bool> HasOpenCashRegisterAsync(Guid userId, CancellationToken ct);
    Task<CashRegister?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<PagedResult<CashRegister>> GetAllAsync(
        Guid? userId,
        CashRegisterStatus? status,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize,
        CancellationToken ct);
}