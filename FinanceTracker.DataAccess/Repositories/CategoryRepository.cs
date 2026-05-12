using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class CategoryRepository(FinanceTrackerContext context) : Repository<Category>(context), ICategoryRepository
{
    private FinanceTrackerContext FinanceTrackerContext => (FinanceTrackerContext)Context;

    public async Task<IEnumerable<Category>> GetByUserAsync(Guid userId)
    {
        return await FinanceTrackerContext.Categories
            .Where(category => category.UserId == userId)
            .OrderBy(category => category.Name)
            .ToListAsync();
    }
}
