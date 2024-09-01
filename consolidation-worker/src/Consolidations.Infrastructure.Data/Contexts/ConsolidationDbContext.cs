using Consolidations.Domain.Entities;
using Consolidations.Infrastructure.Data.Configurations;
using Consolidations.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Consolidations.Infrastructure.Data.Contexts;

public class ConsolidationDbContext(DbContextOptions<ConsolidationDbContext> options) : DbContext(options)
{
    public DbSet<ConsolidatedTransaction> ConsolidatedTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ConsolidatedTransaction>(new ConsolidatedTransactionConfiguration().Configure);

        modelBuilder.HasDefaultSchema("domain");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
    }
}