using AutoMapper;
using FinanceTracker.UI.Models;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.FinancialOperations;

public partial class Edit : ComponentBase
{
    [Parameter] public string Id { get; set; } = string.Empty;
    
    private Guid FinancialOperationId => Guid.TryParse(Id, out var id) ? id : Guid.Empty;
    private FinancialOperationRequest? _operation;
    private string? _errorMessage;
    private List<BalanceChangeResponse> _unusedBalanceChanges = [];
    
    [Inject] private IBalanceChangeService BalanceChangeService { get; set; } = null!;
    [Inject] private IFinancialOperationService FinancialOperationService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IMapper Mapper { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        if (FinancialOperationId == Guid.Empty) 
        {
            _errorMessage ="Invalid Balance Change ID";
            return;
        }
        try
        {
            var response = await FinancialOperationService.GetAsync(FinancialOperationId);
            if (response is not null)
            {
                _operation = Mapper.Map<FinancialOperationRequest>(response);
                _unusedBalanceChanges = (await BalanceChangeService.GetUnusedAsync()).ToList();
                _unusedBalanceChanges.Add(await BalanceChangeService.GetAsync(_operation.BalanceChangeId));
            }
            else
            {
                _errorMessage = "Financial operation not found";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error loading financial operation: {ex.Message}";
        }
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            var response = await FinancialOperationService.UpdateAsync(FinancialOperationId, _operation);

            if (response.IsSuccessStatusCode)
            {
                GoToListing();
            }
            else
            {
                _errorMessage = "Failed to update financial operation";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error updating financial operation: {ex.Message}";
        }
    }

    private void GoToListing()
    {
        NavigationManager.NavigateTo("/financial-operations");
    }
}