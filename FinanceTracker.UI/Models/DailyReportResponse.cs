namespace FinanceTracker.UI.Models;

public record DailyReportResponse(
    DateTime Date, 
    decimal TotalIncome, 
    decimal TotalExpenses,
    IEnumerable<FinancialOperationResponse> Operations);