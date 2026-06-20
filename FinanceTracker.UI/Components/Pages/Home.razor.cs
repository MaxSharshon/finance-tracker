using System.Globalization;
using FinanceTracker.Contracts.Budgets;
using FinanceTracker.Contracts.Categories;
using FinanceTracker.Contracts.Enums;
using FinanceTracker.Contracts.FinancialOperations;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages;

public partial class Home
{
    private readonly DateTime _periodStart = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    private readonly DateTime _periodEnd = DateTime.Today;

    private List<FinancialOperationResponse> _operations = [];
    private List<CategoryResponse> _categories = [];
    private List<BudgetResponse> _budgets = [];
    private bool _isLoading = true;
    private string? _errorMessage;

    [Inject] private IFinancialOperationService FinancialOperationService { get; set; } = null!;
    [Inject] private ICategoryService CategoryService { get; set; } = null!;
    [Inject] private IBudgetService BudgetService { get; set; } = null!;
    [Inject] private IAuthClientService AuthClient { get; set; } = null!;

    private decimal MonthlyIncome => _operations
        .Where(operation => GetOperationType(operation.CategoryId) == OperationType.Income)
        .Sum(operation => operation.Amount);

    private decimal MonthlyExpenses => _operations
        .Where(operation => GetOperationType(operation.CategoryId) == OperationType.Expense)
        .Sum(operation => operation.Amount);

    private decimal NetFlow => MonthlyIncome - MonthlyExpenses;
    private decimal CurrentBalance => NetFlow;

    private int IncomeOperationCount => _operations.Count(operation =>
        GetOperationType(operation.CategoryId) == OperationType.Income);

    private int ExpenseOperationCount => _operations.Count(operation =>
        GetOperationType(operation.CategoryId) == OperationType.Expense);

    private IEnumerable<FinancialOperationResponse> RecentOperations => _operations
        .OrderByDescending(operation => operation.Date)
        .Take(5);

    private IReadOnlyList<BudgetUsageItem> BudgetUsageItems => _budgets
        .Select(CreateBudgetUsageItem)
        .ToList();

    private int BudgetsNearLimitCount => BudgetUsageItems.Count(item => item.UsagePercent >= 80);

    protected override async Task OnInitializedAsync()
    {
        await LoadDashboardAsync();
    }

    private async Task LoadDashboardAsync()
    {
        if (!AuthClient.IsAuthenticated)
        {
            _isLoading = false;
            return;
        }

        try
        {
            _isLoading = true;
            _errorMessage = null;

            _categories = (await CategoryService.GetAllAsync()).ToList();
            _budgets = (await BudgetService.GetAllAsync()).ToList();
            _operations = (await FinancialOperationService.GetAllAsync(
                _periodStart,
                _periodEnd,
                null,
                null,
                null)).ToList();
        }
        catch (Exception exception)
        {
            _errorMessage = $"Failed to load dashboard: {exception.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private BudgetUsageItem CreateBudgetUsageItem(BudgetResponse budget)
    {
        var spent = _operations
            .Where(operation => operation.BudgetId == budget.Id)
            .Where(operation => GetOperationType(operation.CategoryId) == OperationType.Expense)
            .Sum(operation => operation.Amount);

        var limit = budget.LimitAmount ?? 0;
        var usagePercent = limit > 0
            ? Math.Min(spent / limit * 100, 100)
            : 0;

        return new BudgetUsageItem(budget.Name, spent, limit, usagePercent);
    }

    private OperationType GetOperationType(Guid categoryId)
    {
        var category = _categories.FirstOrDefault(item => item.Id == categoryId);

        return Enum.TryParse<OperationType>(category?.OperationType, true, out var operationType)
            ? operationType
            : OperationType.Expense;
    }

    private string GetCategoryName(Guid categoryId)
    {
        return _categories.FirstOrDefault(item => item.Id == categoryId)?.Name ?? "Uncategorized";
    }

    private static string GetCategoryBadgeClass(OperationType operationType)
    {
        return operationType == OperationType.Income ? "success" : "danger";
    }

    private static string GetAmountClass(OperationType operationType)
    {
        return operationType == OperationType.Income ? "income" : "expense";
    }

    private static string FormatSignedMoney(decimal amount, OperationType operationType)
    {
        var sign = operationType == OperationType.Income ? "+" : "-";
        return $"{sign}{FormatMoney(amount)}";
    }

    private static string FormatMoney(decimal amount)
    {
        return amount.ToString("C", CultureInfo.GetCultureInfo("uk-UA"));
    }

    private sealed record BudgetUsageItem(string Name, decimal Spent, decimal Limit, decimal UsagePercent);
}
