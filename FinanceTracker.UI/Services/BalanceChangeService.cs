using FinanceTracker.UI.Models;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FinanceTracker.UI.Services;

public class BalanceChangeService(HttpClient client)
    : Service<BalanceChangeRequest, BalanceChangeResponse>(client, ENDPOINT), IBalanceChangeService
{
    private const string ENDPOINT = "BalanceChange";
    
    public async Task<IEnumerable<BalanceChangeResponse>> GetUnusedAsync()
    {
        return await client.GetFromJsonAsync<IEnumerable<BalanceChangeResponse>>($"{ENDPOINT}/unused")
               ?? throw new InvalidOperationException("Failed to get unused balance changes");
    }
}