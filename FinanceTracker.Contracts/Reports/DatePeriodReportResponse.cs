using FinanceTracker.API.Contracts.FinancialOperations;

namespace FinanceTracker.API.Contracts.Reports;

public record DatePeriodReportResponse(
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal NetTotal,
    int OperationsCount,
    IEnumerable<FinancialOperationResponse> Operations,
    IEnumerable<CategoryReportSummaryResponse> Categories,
    IEnumerable<BudgetReportSummaryResponse> Budgets);