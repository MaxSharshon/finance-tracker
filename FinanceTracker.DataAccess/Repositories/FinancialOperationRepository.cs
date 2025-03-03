using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;

namespace FinanceTracker.DataAccess.Repositories;

public class FinancialOperationRepository(FinanceTrackerContext context)
    : Repository<FinancialOperation>(context), IFinancialOperationRepository
{
    public FinanceTrackerContext FinanceTrackerContext => Context as FinanceTrackerContext;
}