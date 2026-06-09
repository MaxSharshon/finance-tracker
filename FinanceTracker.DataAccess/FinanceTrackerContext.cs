using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess;

public class FinanceTrackerContext(DbContextOptions<FinanceTrackerContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<OperationTag> OperationTags { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetUser> BudgetUsers { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<FinancialOperation> FinancialOperations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new CategoryConfig());
        modelBuilder.ApplyConfiguration(new TagConfig());
        modelBuilder.ApplyConfiguration(new OperationTagConfig());
        modelBuilder.ApplyConfiguration(new BudgetConfig());
        modelBuilder.ApplyConfiguration(new BudgetUserConfig());
        modelBuilder.ApplyConfiguration(new NotificationConfig());
        modelBuilder.ApplyConfiguration(new FinancialOperationConfig());
        base.OnModelCreating(modelBuilder);
    }
}
