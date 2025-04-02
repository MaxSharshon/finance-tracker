namespace FinanceTracker.UI.Models;

public class DatePeriodFilter
{
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime EndDate { get; set; } = DateTime.Today;
}