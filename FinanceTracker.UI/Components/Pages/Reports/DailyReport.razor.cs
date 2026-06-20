using System.Globalization;
using FinanceTracker.Contracts.Budgets;
using FinanceTracker.Contracts.Categories;
using FinanceTracker.Contracts.Enums;
using FinanceTracker.Contracts.Reports;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Reports;

public partial class DailyReport
{
    private readonly ReportDateFormModel _form = new();
    private DailyReportResponse? _report;
    private List<CategoryResponse> _categories = [];
    private List<BudgetResponse> _budgets = [];
    private string? _errorMessage;

    [Inject] private IReportsService ReportsService { get; set; } = null!;
    [Inject] private ICategoryService CategoryService { get; set; } = null!;
    [Inject] private IBudgetService BudgetService { get; set; } = null!;

    private decimal NetTotal => (_report?.TotalIncome ?? 0) - (_report?.TotalExpenses ?? 0);
    private int OperationsCount => _report?.Operations.Count() ?? 0;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _categories = (await CategoryService.GetAllAsync()).ToList();
            _budgets = (await BudgetService.GetAllAsync()).ToList();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load report lookup data: {ex.Message}";
        }
    }

    private async Task FetchReport()
    {
        try
        {
            _errorMessage = null;
            _report = await ReportsService.GetDailyReportAsync(_form.Date);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to generate report: {ex.Message}";
        }
    }

    private string GetCategoryName(Guid categoryId)
    {
        return _categories.FirstOrDefault(category => category.Id == categoryId)?.Name ?? "Unknown";
    }

    private string GetBudgetName(Guid? budgetId)
    {
        return budgetId.HasValue
            ? _budgets.FirstOrDefault(budget => budget.Id == budgetId.Value)?.Name ?? "Unknown"
            : "Personal";
    }

    private OperationType GetOperationType(Guid categoryId)
    {
        var category = _categories.FirstOrDefault(item => item.Id == categoryId);
        return Enum.TryParse<OperationType>(category?.OperationType, true, out var type)
            ? type
            : OperationType.Expense;
    }

    private static string GetAmountClass(OperationType type) =>
        type == OperationType.Income ? "income" : "expense";

    private static string FormatSignedMoney(decimal amount, OperationType type)
    {
        var sign = type == OperationType.Income ? "+" : "-";
        return $"{sign}{FormatMoney(amount)}";
    }

    private static string FormatMoney(decimal amount) =>
        amount.ToString("C", CultureInfo.GetCultureInfo("uk-UA"));

    private sealed class ReportDateFormModel
    {
        public DateTime Date { get; set; } = DateTime.Today;
    }
}
