using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.DataAccess.Configurations;

public class BudgetUserConfig : IEntityTypeConfiguration<BudgetUser>
{
    public void Configure(EntityTypeBuilder<BudgetUser> builder)
    {
        builder.HasKey(budgetUser => new { budgetUser.BudgetId, budgetUser.UserId });

        builder.HasOne(budgetUser => budgetUser.Budget)
            .WithMany(budget => budget.BudgetUsers)
            .HasForeignKey(budgetUser => budgetUser.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(budgetUser => budgetUser.User)
            .WithMany(user => user.BudgetUsers)
            .HasForeignKey(budgetUser => budgetUser.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
