using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserContext _currentUserContext;

    public AuditSaveChangesInterceptor(ICurrentUserContext currentUserContext)
    {
        _currentUserContext = currentUserContext;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not { } context)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var userId = _currentUserContext.UserId ?? Guid.Empty;

        foreach (var entry in context.ChangeTracker.Entries().ToList())
        {
            if (entry.Entity is AuditLog)
                continue;

            AuditLog? auditLog = entry.State switch
            {
                EntityState.Added => new AuditLog(
                    userId, "Created", entry.Metadata.ClrType.Name,
                    oldValue: null,
                    newValue: BuildSnapshot(entry.Properties, current: true)),

                EntityState.Modified when entry.Properties.Any(p => p.IsModified) => new AuditLog(
                    userId, "Updated", entry.Metadata.ClrType.Name,
                    oldValue: BuildSnapshot(entry.Properties.Where(p => p.IsModified), current: false),
                    newValue: BuildSnapshot(entry.Properties.Where(p => p.IsModified), current: true)),

                EntityState.Deleted => new AuditLog(
                    userId, "Deleted", entry.Metadata.ClrType.Name,
                    oldValue: BuildSnapshot(entry.Properties, current: false),
                    newValue: null),

                _ => null
            };

            if (auditLog is not null)
                await context.AddAsync(auditLog, cancellationToken);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static Dictionary<string, object?> BuildSnapshot(IEnumerable<PropertyEntry> properties, bool current)
    {
        return properties.ToDictionary(
            p => p.Metadata.Name,
            p => current ? p.CurrentValue : p.OriginalValue);
    }
}