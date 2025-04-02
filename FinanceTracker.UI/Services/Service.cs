using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class Service<TRequest, TResponse>(HttpClient client, string endpoint) 
    : IService<TRequest, TResponse>
{
    public async Task<IEnumerable<TResponse>> GetAllAsync()
    {
        return await client.GetFromJsonAsync<IEnumerable<TResponse>>(endpoint)
               ?? throw new InvalidOperationException($"Failed to retrieve data from {endpoint}.");
    }

    public async Task<TResponse?> GetAsync(Guid id)
    {
        return await client.GetFromJsonAsync<TResponse>($"{endpoint}/{id}");
    }

    public async Task<HttpResponseMessage> AddAsync(TRequest request)
    {
        return await client.PostAsJsonAsync(endpoint, request);
    }

    public async Task<HttpResponseMessage> UpdateAsync(Guid id, TRequest request)
    {
        return await client.PutAsJsonAsync($"{endpoint}/{id}", request);
    }

    public async Task<HttpResponseMessage> DeleteAsync(Guid id)
    {
        return await client.DeleteAsync($"{endpoint}/{id}");
    }
}