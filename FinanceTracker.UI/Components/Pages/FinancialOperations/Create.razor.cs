using FinanceTracker.Contracts.Budgets;
using FinanceTracker.Contracts.Categories;
using FinanceTracker.Contracts.FinancialOperations;
using FinanceTracker.Contracts.Tags;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.FinancialOperations;

public partial class Create
{
    private readonly FinancialOperationFormModel _operation = new();
    private List<CategoryResponse> _categories = [];
    private List<BudgetResponse> _budgets = [];
    private List<TagResponse> _tags = [];
    private string? _errorMessage;


    [Inject] private IFinancialOperationService FinancialOperationService { get; set; } = null!;
    [Inject] private ICategoryService CategoryService { get; set; } = null!;
    [Inject] private IBudgetService BudgetService { get; set; } = null!;
    [Inject] private ITagService TagService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _categories = (await CategoryService.GetAllAsync()).ToList();
            _budgets = (await BudgetService.GetAllAsync()).ToList();
            _tags = (await TagService.GetAllAsync()).ToList();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load form data: {ex.Message}";
        }
    }

    private async Task HandleValidSubmit()
    {
        if (!Guid.TryParse(_operation.CategoryId, out var categoryId))
        {
            _errorMessage = "Select category.";
            return;
        }

        var budgetId = Guid.TryParse(_operation.BudgetId, out var parsedBudgetId)
            ? parsedBudgetId
            : (Guid?)null;

        try
        {
            var request = new FinancialOperationRequest(
                categoryId,
                budgetId,
                _operation.Amount,
                _operation.Date,
                _operation.Description,
                _operation.TagIds.ToList());

            var response = await FinancialOperationService.AddAsync(request);

            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("/financial-operations");
                return;
            }

            _errorMessage = "Failed to create financial operation.";
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error creating financial operation: {ex.Message}";
        }
    }
}

    private void ToggleTag(Guid tagId, bool isSelected)
    {
        if (isSelected)
        {
            _operation.TagIds.Add(tagId);
            return;
        }

        _operation.TagIds.Remove(tagId);
    }

    private void GoToListing()
    {
        NavigationManager.NavigateTo("/financial-operations");
    }

    private sealed class FinancialOperationFormModel
    {
        public string CategoryId { get; set; } = string.Empty;
        public string? BudgetId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public string Description { get; set; } = string.Empty;
        public HashSet<Guid> TagIds { get; } = [];
    }
}
