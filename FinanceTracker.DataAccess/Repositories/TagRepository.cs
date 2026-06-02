using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class TagRepository(FinanceTrackerContext context) : Repository<Tag>(context), ITagRepository
{
    private FinanceTrackerContext FinanceTrackerContext => (FinanceTrackerContext)Context;

    public async Task<Tag?> GetByIdAsync(Guid id, Guid userId)
    {
        return await FinanceTrackerContext.Tags
            .FirstOrDefaultAsync(tag => tag.Id == id && tag.UserId == userId);
    }
    
    public async Task<IEnumerable<Tag>> GetByUserAsync(Guid userId)
    {
        return await FinanceTrackerContext.Tags
            .Where(tag => tag.UserId == userId)
            .OrderBy(tag => tag.Name)
            .ToListAsync();
    }
}
