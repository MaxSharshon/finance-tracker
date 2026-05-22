using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface IFinancialOperationRepository : IRepository<FinancialOperation>
{
    Task<FinancialOperation?> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<FinancialOperation>> GetAllAsync(Guid userId);
    Task<IEnumerable<FinancialOperation>> GetByDateAsync(DateTime date, Guid userId);
    Task<IEnumerable<FinancialOperation>> GetByPeriodAsync(DateTime startDate, DateTime endDate, Guid userId);
}