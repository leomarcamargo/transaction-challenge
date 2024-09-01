using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transactions.Domain.Entities;

namespace Transactions.Infrastructure.Data.Configurations;

public class TransactionConfiguration : EntityConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        base.Configure(builder);

        builder.ToTable("Transactions");

        builder.Property(t => t.Date)
            .IsRequired();

        builder.Property(t => t.Total)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.Type)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(250);
    }
}