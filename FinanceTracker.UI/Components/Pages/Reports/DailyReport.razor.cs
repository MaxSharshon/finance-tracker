using FinanceTracker.UI.Models;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Reports;

public partial class DailyReport
{
    private DateTime _selectedDate = DateTime.Now;
    private DailyReportResponse? _report;
    private string? _errorMessage;

    [Inject] private IReportsService Service { get; set; } = null!;
    
    private async Task FetchReport()
    {
        try
        {
            _report = await Service.GetDailyReportAsync(_selectedDate);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error fetch report: {ex.Message}";
        }
    }
}