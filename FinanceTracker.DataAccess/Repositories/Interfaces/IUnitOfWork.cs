namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ICategoryRepository Categories { get; }
    ITagRepository Tags { get; }
    IBudgetRepository Budgets { get; }
    INotificationRepository Notifications { get; }
    IFinancialOperationRepository FinancialOperations { get; }
    
    Task<int> CompleteAsync();
}
