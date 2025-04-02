using FinanceTracker.UI.Models;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.FinancialOperations;

public partial class Listing
{
    private List<FinancialOperationResponse>? _operations;
    private string? _errorMessage;

    [Inject] private IFinancialOperationService Service { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await FetchOperationsAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error loading financial operations: {ex.Message}";
        }
    }

    private async Task FetchOperationsAsync()
    {
        _operations = (await Service.GetAllAsync()).ToList();
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
            var response = await Service.DeleteAsync(id);
            
            if (response.IsSuccessStatusCode)
            {
                _operations!.RemoveAll(operation => operation.Id == id);
            }
            else
            {
                _errorMessage = "Failed to delete financial operation.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error deleting financial operation: {ex.Message}";
        }
    }
}