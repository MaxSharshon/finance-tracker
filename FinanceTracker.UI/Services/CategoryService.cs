using FinanceTracker.Contracts.Categories;
using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class CategoryService(HttpClient client)
    : Service<CategoryRequest, CategoryResponse>(client, ENDPOINT), ICategoryService
{
    private const string ENDPOINT = "Categories";
}
