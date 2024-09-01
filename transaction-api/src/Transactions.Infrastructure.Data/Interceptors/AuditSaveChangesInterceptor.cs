using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Transactions.Infrastructure.Core.Abstractions;

namespace Transactions.Infrastructure.Data.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData dbContextEventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (dbContextEventData.Context == null)
            return base.SavingChangesAsync(dbContextEventData, result, cancellationToken);

        SetAuditData(dbContextEventData.Context);

        return base.SavingChangesAsync(dbContextEventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData dbContextEventData, InterceptionResult<int> result)
    {
        if (dbContextEventData.Context == null)
            return base.SavingChanges(dbContextEventData, result);

        SetAuditData(dbContextEventData.Context);

        return base.SavingChanges(dbContextEventData, result);
    }

    private static void SetAuditData(DbContext dbContext)
    {
        var entries = dbContext.ChangeTracker.Entries()
            .Where(x => x is
            {
                Entity: Entity,
                State: EntityState.Added or EntityState.Modified
            });
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((Entity)entry.Entity).CreatedAt = DateTime.UtcNow;
                ((Entity)entry.Entity).CreatedBy = "System";
            }
            else if (entry.State == EntityState.Modified)
            {
                ((Entity)entry.Entity).ModifiedAt = DateTime.UtcNow;
                ((Entity)entry.Entity).ModifiedBy = "System";
            }
        }
    }
}