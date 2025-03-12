namespace FinanceTracker.BusinessLogic.DTOs;

public class DatePeriodReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public IEnumerable<FinancialOperationDto> Operations { get; set; }
}