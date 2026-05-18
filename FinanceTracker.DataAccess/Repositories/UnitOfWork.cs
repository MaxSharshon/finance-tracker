using FinanceTracker.DataAccess.Repositories.Interfaces;

namespace FinanceTracker.DataAccess.Repositories;

public class UnitOfWork(
    FinanceTrackerContext context,
    IUserRepository users,
    ICategoryRepository categories,
    ITagRepository tags,
    IBudgetRepository budgets,
    INotificationRepository notifications,
    IFinancialOperationRepository financialOperations,
    IBalanceChangeRepository balanceChanges) 
    : IUnitOfWork
{
    public IUserRepository Users => users;
    public ICategoryRepository Categories => categories;
    public ITagRepository Tags => tags;
    public IBudgetRepository Budgets => budgets;
    public INotificationRepository Notifications => notifications;
    public IFinancialOperationRepository FinancialOperations => financialOperations;
    public IBalanceChangeRepository BalanceChanges => balanceChanges;

    public async Task<int> CompleteAsync() => await context.SaveChangesAsync();

    public void Dispose() => context.Dispose();
}
