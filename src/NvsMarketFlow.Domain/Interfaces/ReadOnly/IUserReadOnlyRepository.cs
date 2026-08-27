using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface IUserReadOnlyRepository
{
    Task<bool> EmailExists(string email, CancellationToken cancellationToken);
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<User>> GetAllAsync(
        string? name,
        int page,
        int pageSize,
        CancellationToken ct);
}