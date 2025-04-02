namespace FinanceTracker.UI.Models;

public record DatePeriodReportResponse(
    DateTime StartDate, 
    DateTime EndDate, 
    decimal TotalIncome, 
    decimal TotalExpenses,
    IEnumerable<FinancialOperationResponse> Operations);