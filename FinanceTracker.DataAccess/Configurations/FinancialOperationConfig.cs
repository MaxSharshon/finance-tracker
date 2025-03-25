using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.DataAccess.Configurations;

public class FinancialOperationConfig : IEntityTypeConfiguration<FinancialOperation>
{
    public void Configure(EntityTypeBuilder<FinancialOperation> builder)
    {
        builder.HasKey(operation => operation.Id);
        
        builder
            .Property(operation => operation.Id)
            .HasDefaultValueSql("newid()");
        
        builder
            .Property(operation => operation.Date)
            .IsRequired();

        builder
            .Property(operation => operation.BalanceChangeId)
            .IsRequired();

        builder
            .HasOne(operation => operation.BalanceChange)
            .WithOne()
            .HasForeignKey<FinancialOperation>(operation => operation.BalanceChangeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}