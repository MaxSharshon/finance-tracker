using FinanceTracker.Contracts.FinancialOperations;

namespace FinanceTracker.Contracts.Reports;

public record DailyReportResponse(
    DateTime Date,
    decimal TotalIncome,
    decimal TotalExpenses,
    IEnumerable<FinancialOperationResponse> Operations);