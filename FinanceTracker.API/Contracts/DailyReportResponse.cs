using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.API.Contracts;

public record DailyReportResponse(
    DateTime Date,
    decimal TotalIncome,
    decimal TotalExpenses,
    IEnumerable<FinancialOperationResponse> Operations);