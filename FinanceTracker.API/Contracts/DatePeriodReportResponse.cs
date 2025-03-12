namespace FinanceTracker.API.Contracts;

public record DatePeriodReportResponse(
    string StartDate,
    string EndDate,
    decimal TotalIncome,
    decimal TotalExpenses,
    IEnumerable<FinancialOperationResponse> Operations);