using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;

namespace FinanceTracker.DataAccess.Repositories;

public class BalanceChangeRepository(FinanceTrackerContext context)
    : Repository<BalanceChange>(context), IBalanceChangeRepository
{
    public FinanceTrackerContext FinanceTrackerContext => Context as FinanceTrackerContext;
}