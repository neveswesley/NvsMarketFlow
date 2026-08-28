using System.Text.Json;

namespace NvsMarketFlow.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }
    public string Action { get; private set; }
    public string Entity { get; private set; }
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }

    public DateTime Date { get; private set; }

    public AuditLog()
    {
    }

    public AuditLog(Guid userId, string action, string entity, object? oldValue, object? newValue)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Action = action;
        Entity = entity;
        OldValue = oldValue is null ? null : JsonSerializer.Serialize(oldValue);
        NewValue = newValue is null ? null : JsonSerializer.Serialize(newValue);
        Date = DateTime.UtcNow;
    }
}