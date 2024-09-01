using Consolidations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Consolidations.Infrastructure.Data.Configurations;

public class ConsolidatedTransactionConfiguration : EntityConfiguration<ConsolidatedTransaction>
{
    public override void Configure(EntityTypeBuilder<ConsolidatedTransaction> builder)
    {
        base.Configure(builder);

        builder.ToTable("ConsolidatedTransactions");

        builder.Property(ct => ct.TransactionId)
            .IsRequired();

        builder.Property(ct => ct.Date)
            .IsRequired();

        builder.Property(ct => ct.Total)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(ct => ct.DailyBalance)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(ct => ct.Type)
            .IsRequired();

        builder.Property(ct => ct.Description)
            .HasMaxLength(250);
    }
}