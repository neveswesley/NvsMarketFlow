using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface ICashRegisterWriteOnlyRepository
{
    Task<CashRegister> CreateAsync(CashRegister cashRegister, CancellationToken ct);
    Task UpdateAsync(CashRegister cashRegister, CancellationToken ct);
}