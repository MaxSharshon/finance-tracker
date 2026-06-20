using FinanceTracker.Contracts.Tags;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Tags;

public partial class Listing
{
    private readonly TagFormModel _form = new();
    private List<TagResponse> _tags = [];
    private Guid? _editingId;
    private Guid? _deletingId;
    private bool _isLoading = true;
    private bool _isSaving;
    private string? _errorMessage;
    private string? _successMessage;

    [Inject] private ITagService TagService { get; set; } = null!;

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
            _tags = (await TagService.GetAllAsync()).ToList();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load tags: {ex.Message}";
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
        _errorMessage = null;
        _successMessage = null;
    }

    private void StartEdit(TagResponse tag)
    {
        _editingId = tag.Id;
        _form.Name = tag.Name;
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

            var request = new TagRequest(_form.Name);
            var response = _editingId.HasValue
                ? await TagService.UpdateAsync(_editingId.Value, request)
                : await TagService.AddAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to save tag.";
                return;
            }

            var successMessage = _editingId.HasValue ? "Tag updated." : "Tag created.";
            StartCreate();
            _successMessage = successMessage;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to save tag: {ex.Message}";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task DeleteSelectedAsync()
    {
        if (!_editingId.HasValue)
        {
            return;
        }

        await DeleteAsync(_editingId.Value);
    }

    private async Task DeleteAsync(Guid id)
    {
        try
        {
            _isSaving = true;
            _deletingId = id;
            _errorMessage = null;
            _successMessage = null;

            var response = await TagService.DeleteAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to delete tag.";
                return;
            }

            StartCreate();
            _successMessage = "Tag deleted.";
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to delete tag: {ex.Message}";
        }
        finally
        {
            _deletingId = null;
            _isSaving = false;
        }
    }

    private sealed class TagFormModel
    {
        public string Name { get; set; } = string.Empty;
    }
}
