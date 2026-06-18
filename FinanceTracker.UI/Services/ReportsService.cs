using FinanceTracker.Contracts.Reports;
using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class ReportsService(HttpClient client) : IReportsService
{
    private const string DAILY_REPORT_ENDPOINT = "daily-report";
    private const string DATE_PERIOD_REPORT_ENDPOINT = "date-period-report";

    public async Task<DailyReportResponse> GetDailyReportAsync(DateTime date)
    {
        return await client.GetFromJsonAsync<DailyReportResponse>($"{DAILY_REPORT_ENDPOINT}?date={date:yyyy-MM-dd}")
               ?? throw new InvalidOperationException("Failed to retrieve daily report");
    }

    public async Task<DatePeriodReportResponse> GetDatePeriodReportAsync(DateTime startDate, DateTime endDate)
    {
        return await client.GetFromJsonAsync<DatePeriodReportResponse>(
            $"{DATE_PERIOD_REPORT_ENDPOINT}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}")
            ?? throw new InvalidOperationException("Failed to retrieve date period report");
    }
}
