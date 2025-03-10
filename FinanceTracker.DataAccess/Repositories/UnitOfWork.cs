using FinanceTracker.DataAccess.Repositories.Interfaces;

namespace FinanceTracker.DataAccess.Repositories;

public class UnitOfWork(
    FinanceTrackerContext context,
    IFinancialOperationRepository financialOperations,
    IBalanceChangeRepository balanceChanges) 
    : IUnitOfWork
{
    public IFinancialOperationRepository FinancialOperations => financialOperations;
    public IBalanceChangeRepository BalanceChanges => balanceChanges;

    public async Task<int> CompleteAsync() => await context.SaveChangesAsync();

    public void Dispose() => context.Dispose();
}