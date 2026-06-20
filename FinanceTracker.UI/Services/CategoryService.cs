using FinanceTracker.Contracts.Categories;
using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class CategoryService(HttpClient client)
    : Service<CategoryRequest, CategoryResponse>(client, ENDPOINT), ICategoryService
{
    private const string ENDPOINT = "Categories";

    public async Task<CategorySuggestionResponse?> SuggestAsync(CategorySuggestionRequest request)
    {
        var response = await client.PostAsJsonAsync($"{ENDPOINT}/suggest", request);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CategorySuggestionResponse>();
    }
}
