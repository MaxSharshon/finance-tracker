using FinanceTracker.Contracts.Budgets;
using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class BudgetService(HttpClient client)
    : Service<BudgetRequest, BudgetResponse>(client, ENDPOINT), IBudgetService
{
    private const string ENDPOINT = "Budgets";
}
