using FinanceTracker.API.Contracts.FinancialOperations;

namespace FinanceTracker.API.Contracts.Reports;

public record DatePeriodReportResponse(
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalIncome,
    decimal TotalExpenses,
    IEnumerable<FinancialOperationResponse> Operations);