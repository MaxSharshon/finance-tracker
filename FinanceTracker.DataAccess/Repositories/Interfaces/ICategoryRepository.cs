using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetByUserAsync(Guid userId);
}
