using FinanceTracker.Contracts.Budgets;
using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class BudgetService(HttpClient client)
    : Service<BudgetRequest, BudgetResponse>(client, ENDPOINT), IBudgetService
{
    private const string ENDPOINT = "Budgets";

    public async Task<IEnumerable<BudgetMemberResponse>> GetMembersAsync(Guid budgetId)
    {
        return await client.GetFromJsonAsync<IEnumerable<BudgetMemberResponse>>($"{ENDPOINT}/{budgetId}/members")
               ?? throw new InvalidOperationException("Failed to retrieve budget members.");
    }

    public async Task<HttpResponseMessage> AddMemberAsync(Guid budgetId, BudgetMemberRequest request)
    {
        return await client.PostAsJsonAsync($"{ENDPOINT}/{budgetId}/members", request);
    }

    public async Task<HttpResponseMessage> RemoveMemberAsync(Guid budgetId, Guid memberUserId)
    {
        return await client.DeleteAsync($"{ENDPOINT}/{budgetId}/members/{memberUserId}");
    }
}
