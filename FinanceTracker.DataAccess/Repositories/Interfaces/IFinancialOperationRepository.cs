using System.Linq.Expressions;
using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface IFinancialOperationRepository : IRepository<FinancialOperation>
{
    Task<IEnumerable<FinancialOperation>> GetByDateWithBalanceChangeAsync(DateTime date);
    Task<IEnumerable<FinancialOperation>> GetByDateWithBalanceChangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<FinancialOperation>> FindWithBalanceChangeAsync(Expression<Func<FinancialOperation, bool>> predicate);
}