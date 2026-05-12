using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.DataAccess.Configurations;

public class BudgetConfig : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(budget => budget.Id);

        builder.Property(budget => budget.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(budget => budget.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(budget => budget.LimitAmount)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(budget => budget.OwnerUser)
            .WithMany(user => user.OwnedBudgets)
            .HasForeignKey(budget => budget.OwnerUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
