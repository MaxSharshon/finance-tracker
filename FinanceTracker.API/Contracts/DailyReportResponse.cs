namespace FinanceTracker.API.Contracts;

public record DailyReportResponse(
    string Date,
    decimal TotalIncome,
    decimal TotalExpenses,
    IEnumerable<FinancialOperationResponse> Operations);