using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface ITagRepository : IRepository<Tag>
{
    Task<Tag?> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<Tag>> GetByUserAsync(Guid userId);
}
