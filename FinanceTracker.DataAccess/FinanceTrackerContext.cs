using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess;

public class FinanceTrackerContext(DbContextOptions<FinanceTrackerContext> options) : DbContext(options)
{
    public DbSet<BalanceChange> BalanceChanges { get; set; }
    public DbSet<FinancialOperation> FinancialOperations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FinancialOperationConfig());
        modelBuilder.ApplyConfiguration(new BalanceChangeConfig());
        
        base.OnModelCreating(modelBuilder);
    }
}