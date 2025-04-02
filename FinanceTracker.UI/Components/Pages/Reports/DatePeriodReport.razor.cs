using FinanceTracker.UI.Models;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Reports;

public partial class DatePeriodReport : ComponentBase
{
    private DatePeriodFilter _filter = new DatePeriodFilter();
    private DatePeriodReportResponse? _report;
    private string? _errorMessage;

    [Inject] private IReportsService Service { get; set; } = null!;

    private async Task FetchReport()
    {
        try
        {
            _report = await Service.GetDatePeriodReportAsync(_filter.StartDate, _filter.EndDate);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error fetching report: {ex.Message}";
        }
    }
}