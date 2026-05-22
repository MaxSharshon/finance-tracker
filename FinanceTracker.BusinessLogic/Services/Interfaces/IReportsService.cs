using FinanceTracker.BusinessLogic.DTOs;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IReportsService
{
    Task<DailyReportDto> GetDailyReportAsync(DateTime date, Guid userId);
    Task<DatePeriodReportDto> GetDatePeriodReportAsync(DateTime startDate, DateTime endDate, Guid userId);
}