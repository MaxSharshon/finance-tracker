using FinanceTracker.API.Contracts.FinancialOperations;

namespace FinanceTracker.API.Contracts.Reports;

public record DailyReportResponse(
    DateTime Date,
    decimal TotalIncome,
    decimal TotalExpenses,
    IEnumerable<FinancialOperationResponse> Operations);