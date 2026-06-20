using System.Globalization;
using FinanceTracker.Contracts.Budgets;
using FinanceTracker.Contracts.Categories;
using FinanceTracker.Contracts.Enums;
using FinanceTracker.Contracts.FinancialOperations;
using FinanceTracker.Contracts.Tags;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.FinancialOperations;

public partial class Listing
{
    private readonly OperationFilterModel _filter = new();
    private List<FinancialOperationResponse> _operations = [];
    private List<CategoryResponse> _categories = [];
    private List<BudgetResponse> _budgets = [];
    private List<TagResponse> _tags = [];
    private bool _isLoading = true;
    private Guid? _deletingId;
    private string? _errorMessage;

    [Inject] private IFinancialOperationService FinancialOperationService { get; set; } = null!;
    [Inject] private ICategoryService CategoryService { get; set; } = null!;
    [Inject] private IBudgetService BudgetService { get; set; } = null!;
    [Inject] private ITagService TagService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private decimal IncomeTotal => _operations
        .Where(operation => GetOperationType(operation.CategoryId) == OperationType.Income)
        .Sum(operation => operation.Amount);

    private decimal ExpenseTotal => _operations
        .Where(operation => GetOperationType(operation.CategoryId) == OperationType.Expense)
        .Sum(operation => operation.Amount);

    private decimal NetTotal => IncomeTotal - ExpenseTotal;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _categories = (await CategoryService.GetAllAsync()).ToList();
            _budgets = (await BudgetService.GetAllAsync()).ToList();
            _tags = (await TagService.GetAllAsync()).ToList();
            await LoadOperationsAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load financial operations: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task LoadOperationsAsync()
    {
        _operations = (await FinancialOperationService.GetAllAsync(
            _filter.StartDate,
            _filter.EndDate,
            _filter.CategoryId,
            null,
            _filter.OperationType)).ToList();
    }

    private async Task ApplyFiltersAsync()
    {
        try
        {
            _isLoading = true;
            _errorMessage = null;
            await LoadOperationsAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to apply filters: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void NavigateToCreate()
    {
        NavigationManager.NavigateTo("/financial-operations/create");
    }

    private void NavigateToEdit(Guid id)
    {
        NavigationManager.NavigateTo($"/financial-operations/edit/{id}");
    }

    private async Task DeleteAsync(Guid id)
    {
        try
        {
            _deletingId = id;
            _errorMessage = null;
            var response = await FinancialOperationService.DeleteAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to delete financial operation.";
                return;
            }

            _operations.RemoveAll(operation => operation.Id == id);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error deleting financial operation: {ex.Message}";
        }
        finally
        {
            _deletingId = null;
        }
    }

    private OperationType GetOperationType(Guid categoryId)
    {
        var category = _categories.FirstOrDefault(item => item.Id == categoryId);
        return Enum.TryParse<OperationType>(category?.OperationType, true, out var type)
            ? type
            : OperationType.Expense;
    }

    private string GetCategoryName(Guid categoryId)
    {
        return _categories.FirstOrDefault(item => item.Id == categoryId)?.Name ?? "Unknown";
    }

    private string GetBudgetName(Guid? budgetId)
    {
        return budgetId.HasValue
            ? _budgets.FirstOrDefault(item => item.Id == budgetId.Value)?.Name ?? "Unknown"
            : "Personal";
    }

    private string GetTagNames(IEnumerable<Guid> tagIds)
    {
        var names = tagIds
            .Select(id => _tags.FirstOrDefault(tag => tag.Id == id)?.Name)
            .Where(name => !string.IsNullOrWhiteSpace(name));

        return string.Join(", ", names);
    }

    private static string GetBadgeClass(OperationType type) =>
        type == OperationType.Income ? "success" : "danger";

    private static string GetAmountClass(OperationType type) =>
        type == OperationType.Income ? "income" : "expense";

    private static string FormatSignedMoney(decimal amount, OperationType type)
    {
        var sign = type == OperationType.Income ? "+" : "-";
        return $"{sign}{FormatMoney(amount)}";
    }

    private static string FormatMoney(decimal amount) =>
        amount.ToString("C", CultureInfo.GetCultureInfo("uk-UA"));

    private sealed class OperationFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? CategoryId { get; set; }
        public OperationType? OperationType { get; set; }
    }
}
