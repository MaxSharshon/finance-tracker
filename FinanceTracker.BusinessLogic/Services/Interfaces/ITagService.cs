using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface ITagService : IScopedCrudService<TagDto, Guid>
{
    Task<IEnumerable<TagDto>> GetAllAsync(Guid userId);
}