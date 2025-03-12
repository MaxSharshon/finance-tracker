using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface IFinancialOperationRepository : IRepository<FinancialOperation>
{
    Task<IEnumerable<FinancialOperation>> GetByDateWithBalanceChangeAsync(DateTime date);
}