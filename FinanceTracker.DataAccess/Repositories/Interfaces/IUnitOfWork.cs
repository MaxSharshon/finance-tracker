namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IFinancialOperationRepository FinancialOperations { get; }
    IBalanceChangeRepository BalanceChanges { get; }
    
    int Complete();
}