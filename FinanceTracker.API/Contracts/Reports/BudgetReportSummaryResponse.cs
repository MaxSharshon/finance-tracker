namespace FinanceTracker.API.Contracts.Reports;

public record BudgetReportSummaryResponse(
    Guid BudgetId,
    string BudgetName,
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal NetTotal,
    int OperationsCount);
