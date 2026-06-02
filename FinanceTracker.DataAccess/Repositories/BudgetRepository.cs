using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class BudgetRepository(FinanceTrackerContext context) : Repository<Budget>(context), IBudgetRepository
{
    private FinanceTrackerContext FinanceTrackerContext => (FinanceTrackerContext)Context;

    public async Task<IEnumerable<Budget>> GetByUserAsync(Guid userId)
    {
        return await FinanceTrackerContext.Budgets
            .Include(budget => budget.BudgetUsers)
            .Where(budget => budget.OwnerUserId == userId || budget.BudgetUsers.Any(member => member.UserId == userId))
            .OrderBy(budget => budget.Name)
            .ToListAsync();
    }

    public async Task<Budget?> GetByIdAsync(Guid id, Guid userId)
    {
        return await FinanceTrackerContext.Budgets
            .Include(budget => budget.BudgetUsers)
            .FirstOrDefaultAsync(budget =>
                budget.Id == id && 
                (budget.OwnerUserId == userId ||
                budget.BudgetUsers.Any(member => member.UserId == userId)));
    }
}
