using System.Globalization;
using FinanceTracker.Contracts.Enums;
using FinanceTracker.Contracts.Reports;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Reports;

public partial class DatePeriodReport
{
    private readonly PeriodReportFormModel _form = new();
    private DatePeriodReportResponse? _report;
    private string? _errorMessage;

    [Inject] private IReportsService ReportsService { get; set; } = null!;

    private int OperationsCount => _report?.OperationsCount ?? 0;

    private async Task FetchReport()
    {
        try
        {
            _errorMessage = null;
            _report = await ReportsService.GetDatePeriodReportAsync(_form.StartDate, _form.EndDate);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to generate report: {ex.Message}";
        }
    }

    private static string GetBadgeClass(OperationType type) =>
        type == OperationType.Income ? "success" : "danger";

    private static string GetAmountClass(decimal amount) =>
        amount >= 0 ? "income" : "expense";

    private static string FormatMoney(decimal amount) =>
        amount.ToString("C", CultureInfo.GetCultureInfo("uk-UA"));

    private sealed class PeriodReportFormModel
    {
        public DateTime StartDate { get; set; } = new(DateTime.Today.Year, DateTime.Today.Month, 1);
        public DateTime EndDate { get; set; } = DateTime.Today;
    }
}
