using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface ICategoryService : IScopedCrudService<CategoryDto, Guid>
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(Guid userId);
    Task<CategorySuggestionDto?> SuggestAsync(Guid userId, string description, decimal amount, string operationType);
}
