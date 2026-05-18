using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface IFinancialOperationRepository : IRepository<FinancialOperation>
{
    Task<IEnumerable<FinancialOperation>> GetByDateAsync(DateTime date);
    Task<IEnumerable<FinancialOperation>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
}