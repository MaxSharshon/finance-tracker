using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.DataAccess.Configurations;

public class FinancialOperationConfig : IEntityTypeConfiguration<FinancialOperation>
{
    public void Configure(EntityTypeBuilder<FinancialOperation> builder)
    {
        builder.HasKey(operation => operation.Id);

        builder.Property(operation => operation.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(operation => operation.Date)
            .IsRequired();
        
        builder.Property(operation => operation.CategoryId)
            .IsRequired();

        builder.Property(operation => operation.Amount)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m)
            .IsRequired();

        builder.Property(operation => operation.Description)
            .HasMaxLength(512);

        builder.HasOne(operation => operation.User)
            .WithMany(user => user.FinancialOperations)
            .HasForeignKey(operation => operation.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(operation => operation.Category)
            .WithMany(category => category.FinancialOperations)
            .HasForeignKey(operation => operation.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(operation => operation.Budget)
            .WithMany(budget => budget.FinancialOperations)
            .HasForeignKey(operation => operation.BudgetId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
