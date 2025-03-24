namespace FinanceTracker.API.Contracts;

public record DatePeriodReportResponse(
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalIncome,
    decimal TotalExpenses,
    IEnumerable<FinancialOperationResponse> Operations);