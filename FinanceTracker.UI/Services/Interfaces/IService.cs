namespace FinanceTracker.UI.Services.Interfaces;

public interface IService<in TRequest, TResponse>
{
    Task<IEnumerable<TResponse>> GetAllAsync();
    Task<TResponse?> GetAsync(Guid id);
    Task<HttpResponseMessage> AddAsync(TRequest request);
    Task<HttpResponseMessage> UpdateAsync(Guid id, TRequest request);
    Task<HttpResponseMessage> DeleteAsync(Guid id);
}