using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class FinancialOperationRepository(FinanceTrackerContext context)
    : Repository<FinancialOperation>(context), IFinancialOperationRepository
{
    private FinanceTrackerContext FinanceTrackerContext => (FinanceTrackerContext)Context;

    public async Task<FinancialOperation?> GetByIdAsync(Guid id, Guid userId)
    {
        return await BuildDetailedQuery(userId).FirstOrDefaultAsync(operation => operation.Id == id);
    }

    public async Task<IEnumerable<FinancialOperation>> GetAllAsync(Guid userId)
    {
        return await BuildDetailedQuery(userId)
            .OrderByDescending(operation => operation.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<FinancialOperation>> GetByDateAsync(DateTime date, Guid userId)
    {
        var targetDate = date.Date;
        return await BuildDetailedQuery(userId)
            .Where(operation => operation.Date.Date == targetDate)
            .OrderByDescending(operation => operation.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<FinancialOperation>> GetByPeriodAsync(
        DateTime startDate,
        DateTime endDate,
        Guid userId)
    {
        var start = startDate.Date;
        var end = endDate.Date.AddDays(1);
        return await BuildDetailedQuery(userId)
            .Where(operation => operation.Date >= start && operation.Date < end)
            .OrderByDescending(operation => operation.Date)
            .ToListAsync();
    }

    private IQueryable<FinancialOperation> BuildDetailedQuery(Guid userId)
    {
        return FinanceTrackerContext.FinancialOperations
            .Where(operation => operation.UserId == userId)
            .Include(operation => operation.Category)
            .Include(operation => operation.Budget)
            .Include(operation => operation.User)
            .Include(operation => operation.OperationTags)
            .ThenInclude(tag => tag.Tag);
    }
}
