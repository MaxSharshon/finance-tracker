using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class FinancialOperationRepository(FinanceTrackerContext context)
    : Repository<FinancialOperation>(context), IFinancialOperationRepository
{
    private FinanceTrackerContext FinanceTrackerContext => (FinanceTrackerContext)Context;

    public override async Task<FinancialOperation?> GetAsync(Guid id)
    {
        return await BuildDetailedQuery().FirstOrDefaultAsync(operation => operation.Id == id);
    }

    public override async Task<IEnumerable<FinancialOperation>> GetAllAsync()
    {
        return await BuildDetailedQuery()
            .OrderByDescending(operation => operation.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<FinancialOperation>> GetByDateAsync(DateTime date)
    {
        var targetDate = date.Date;
        return await BuildDetailedQuery()
            .Where(operation => operation.Date.Date == targetDate)
            .OrderByDescending(operation => operation.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<FinancialOperation>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var start = startDate.Date;
        var end = endDate.Date.AddDays(1);
        return await BuildDetailedQuery()
            .Where(operation => operation.Date >= start && operation.Date <= end)
            .OrderByDescending(operation => operation.Date)
            .ToListAsync();
    }

    private IQueryable<FinancialOperation> BuildDetailedQuery()
    {
        return FinanceTrackerContext.FinancialOperations
            .Include(operation => operation.Category)
            .Include(operation => operation.Budget)
            .Include(operation => operation.User)
            .Include(operation => operation.OperationTags)
            .ThenInclude(tag => tag.Tag);
    }
}
