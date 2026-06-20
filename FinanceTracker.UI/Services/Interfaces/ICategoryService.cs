using FinanceTracker.Contracts.Categories;

namespace FinanceTracker.UI.Services.Interfaces;

public interface ICategoryService : IService<CategoryRequest, CategoryResponse>
{
    Task<CategorySuggestionResponse?> SuggestAsync(CategorySuggestionRequest request);
}
