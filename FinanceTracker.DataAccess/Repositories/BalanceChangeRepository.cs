using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class BalanceChangeRepository(FinanceTrackerContext context)
    : Repository<BalanceChange>(context), IBalanceChangeRepository
{
    private FinanceTrackerContext FinanceTrackerContext => Context as FinanceTrackerContext;
    
    public async Task<IEnumerable<BalanceChange>> GetUnusedAsync()
    {
        return await FinanceTrackerContext.BalanceChanges
            .Where(bc => !FinanceTrackerContext.FinancialOperations.Any(fo => fo.BalanceChangeId == bc.Id))
            .ToListAsync();
    }
}