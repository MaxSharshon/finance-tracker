using FinanceTracker.Contracts.FinancialOperations;
using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class FinancialOperationService(HttpClient client)
    : Service<FinancialOperationRequest, FinancialOperationResponse>(client, ENDPOINT), IFinancialOperationService
{
    private const string ENDPOINT = "FinancialOperation";
}
