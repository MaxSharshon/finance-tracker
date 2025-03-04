using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.DataAccess.Configurations;

public class BalanceChangeConfig : IEntityTypeConfiguration<BalanceChange>
{
    public void Configure(EntityTypeBuilder<BalanceChange> builder)
    {
        builder.HasKey(change => change.Id);

        builder
            .Property(change => change.Id)
            .HasDefaultValueSql("newid()")
            .IsRequired();
        
        builder
            .Property(change => change.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder
            .Property(change => change.OperationType)
            .IsRequired();
    }
}