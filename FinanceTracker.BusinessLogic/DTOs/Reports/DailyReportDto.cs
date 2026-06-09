namespace FinanceTracker.BusinessLogic.DTOs.Reports;

public class DailyReportDto
{
    public DateTime Date { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public IEnumerable<FinancialOperationDto> Operations { get; set; }
}