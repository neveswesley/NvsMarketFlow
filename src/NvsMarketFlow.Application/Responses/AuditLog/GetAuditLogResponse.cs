namespace NvsMarketFlow.Application.Responses.AuditLog;

public class GetAuditLogResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime Date { get; set; }
}