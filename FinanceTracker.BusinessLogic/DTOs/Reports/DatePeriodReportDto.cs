namespace FinanceTracker.BusinessLogic.DTOs.Reports;

public class DatePeriodReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetTotal { get; set; }
    public int OperationsCount { get; set; }
    public IEnumerable<FinancialOperationDto> Operations { get; set; } = [];
    public IEnumerable<CategoryReportSummaryDto> Categories { get; set; } = [];
    public IEnumerable<BudgetReportSummaryDto> Budgets { get; set; } = [];
}
