using System.Linq.Expressions;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class FinancialOperationRepository : Repository<FinancialOperation>, IFinancialOperationRepository
{
    public FinancialOperationRepository(FinanceTrackerContext context) : base(context) { }

    private FinanceTrackerContext FinanceTrackerContext => Context as FinanceTrackerContext;
    
    public async Task<IEnumerable<FinancialOperation>> GetByDateWithBalanceChangeAsync(DateTime date)
    {
        return await FindWithBalanceChangeAsync(operation => operation.Date.Date == date);
    }

    public async Task<IEnumerable<FinancialOperation>> GetByDateWithBalanceChangeAsync(DateTime startDate, DateTime endDate)
    {
        return await FindWithBalanceChangeAsync(operation =>
            operation.Date.Date >= startDate && operation.Date.Date <= endDate);
    }

    public async Task<IEnumerable<FinancialOperation>> FindWithBalanceChangeAsync(
        Expression<Func<FinancialOperation, bool>> predicate)
    {
        return await FinanceTrackerContext.FinancialOperations
            .Include(fo => fo.BalanceChange)
            .Where(predicate)
            .ToListAsync();
    }
}