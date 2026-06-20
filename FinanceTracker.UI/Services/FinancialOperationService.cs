using FinanceTracker.Contracts.Enums;
using FinanceTracker.Contracts.FinancialOperations;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace FinanceTracker.UI.Services;

public class FinancialOperationService(HttpClient client)
    : Service<FinancialOperationRequest, FinancialOperationResponse>(client, ENDPOINT), IFinancialOperationService
{
    private const string ENDPOINT = "FinancialOperation";

    public async Task<IEnumerable<FinancialOperationResponse>> GetAllAsync(
        DateTime? startDate,
        DateTime? endDate,
        Guid? categoryId,
        Guid? budgetId,
        OperationType? operationType)
    {
        var query = new Dictionary<string, string?>();

        if (startDate.HasValue)
        {
            query["startDate"] = startDate.Value.ToString("yyyy-MM-dd");
        }

        if (endDate.HasValue)
        {
            query["endDate"] = endDate.Value.ToString("yyyy-MM-dd");
        }

        if (categoryId.HasValue)
        {
            query["categoryId"] = categoryId.Value.ToString();
        }

        if (budgetId.HasValue)
        {
            query["budgetId"] = budgetId.Value.ToString();
        }

        if (operationType.HasValue)
        {
            query["operationType"] = operationType.Value.ToString();
        }

        var url = QueryHelpers.AddQueryString(ENDPOINT, query);

        return await client.GetFromJsonAsync<IEnumerable<FinancialOperationResponse>>(url)
               ?? throw new InvalidOperationException("Failed to retrieve financial operations.");
    }
}
