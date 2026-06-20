using FinanceTracker.Contracts.Categories;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Categories;

public partial class Listing
{
    private readonly CategoryFormModel _form = new();
    private List<CategoryResponse> _categories = [];
    private Guid? _editingId;
    private Guid? _deletingId;
    private bool _isLoading = true;
    private bool _isSaving;
    private string? _errorMessage;
    private string? _successMessage;

    [Inject] private ICategoryService CategoryService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            _isLoading = true;
            _errorMessage = null;
            _categories = (await CategoryService.GetAllAsync()).ToList();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load categories: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void StartCreate()
    {
        _editingId = null;
        _form.Name = string.Empty;
        _form.OperationType = "Expense";
        _errorMessage = null;
        _successMessage = null;
    }

    private void StartEdit(CategoryResponse category)
    {
        _editingId = category.Id;
        _form.Name = category.Name;
        _form.OperationType = category.OperationType;
        _errorMessage = null;
        _successMessage = null;
    }

    private async Task SaveAsync()
    {
        try
        {
            _isSaving = true;
            _errorMessage = null;
            _successMessage = null;

            var request = new CategoryRequest(_form.Name, _form.OperationType);
            var response = _editingId.HasValue
                ? await CategoryService.UpdateAsync(_editingId.Value, request)
                : await CategoryService.AddAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to save category.";
                return;
            }

            var successMessage = _editingId.HasValue
                ? "Category updated."
                : "Category created.";
            StartCreate();
            _successMessage = successMessage;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to save category: {ex.Message}";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task DeleteAsync(Guid id)
    {
        try
        {
            _isSaving = true;
            _deletingId = id;
            _errorMessage = null;
            _successMessage = null;

            var response = await CategoryService.DeleteAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to delete category.";
                return;
            }

            StartCreate();
            _successMessage = "Category deleted.";
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to delete category: {ex.Message}";
        }
        finally
        {
            _deletingId = null;
            _isSaving = false;
        }
    }

    private sealed class CategoryFormModel
    {
        public string Name { get; set; } = string.Empty;
        public string OperationType { get; set; } = "Expense";
    }
}
