using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface ICategoryService : IScopedCrudService<CategoryDto, Guid>
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(Guid userId);
}
