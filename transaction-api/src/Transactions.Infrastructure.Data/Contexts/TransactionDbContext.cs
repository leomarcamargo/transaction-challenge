using Microsoft.EntityFrameworkCore;
using Transactions.Domain.Entities;
using Transactions.Infrastructure.Data.Configurations;
using Transactions.Infrastructure.Data.Interceptors;

namespace Transactions.Infrastructure.Data.Contexts;

public class TransactionDbContext(DbContextOptions<TransactionDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>(new TransactionConfiguration().Configure);

        modelBuilder.HasDefaultSchema("domain");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
    }
}