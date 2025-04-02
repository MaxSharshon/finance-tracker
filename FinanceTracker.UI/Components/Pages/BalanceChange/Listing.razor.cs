using FinanceTracker.UI.Models;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.BalanceChange;

public partial class Listing
{
    private List<BalanceChangeResponse>? _balanceChanges;
    private string? _errorMessage;

    [Inject] private IBalanceChangeService Service { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await FetchAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error loading balance changes: {ex.Message}";
        }
    }

    private async Task FetchAsync()
    {
        _balanceChanges = (await Service.GetAllAsync()).ToList();
    }

    private void NavigateToCreate()
    {
        NavigationManager.NavigateTo("/balance-changes/create");
    }
    
    private void NavigateToEdit(Guid id)
    {
        NavigationManager.NavigateTo($"/balance-changes/edit/{id}");
    }

    private async Task DeleteAsync(Guid id)
    {
        try
        {
            var response = await Service.DeleteAsync(id);
            
            if (response.IsSuccessStatusCode)
            {
                _balanceChanges.RemoveAll(bc => bc.Id == id);
            }
            else
            {
                _errorMessage = "Failed to delete balance change.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error deleting balance change: {ex.Message}";
        }
    }
}