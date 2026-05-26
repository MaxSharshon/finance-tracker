using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<Category>> GetByUserAsync(Guid userId);
}
