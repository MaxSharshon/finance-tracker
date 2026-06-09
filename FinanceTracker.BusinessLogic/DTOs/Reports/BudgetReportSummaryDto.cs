namespace FinanceTracker.BusinessLogic.DTOs.Reports;

public class BudgetReportSummaryDto
{
    public Guid BudgetId { get; set; }
    public string BudgetName { get; set; } = string.Empty;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetTotal { get; set; }
    public int OperationsCount { get; set; }
}
