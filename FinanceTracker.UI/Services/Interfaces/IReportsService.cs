using FinanceTracker.Contracts.Reports;

namespace FinanceTracker.UI.Services.Interfaces;

public interface IReportsService
{
    Task<DailyReportResponse> GetDailyReportAsync(DateTime date);
    Task<DatePeriodReportResponse> GetDatePeriodReportAsync(DateTime startDate, DateTime endDate);
}
