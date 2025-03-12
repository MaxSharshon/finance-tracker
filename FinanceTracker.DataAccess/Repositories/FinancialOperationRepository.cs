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
        return await FinanceTrackerContext.FinancialOperations
            .Where(fo => fo.Date.Date == date)
            .Include(fo => fo.BalanceChange)
            .ToListAsync();
    }
}