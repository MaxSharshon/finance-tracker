using FinanceTracker.UI.Models;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.FinancialOperations;

public partial class Create
{
    private FinancialOperationRequest _newOperation = new();
    private string? _errorMessage;
    private List<BalanceChangeResponse> _unusedBalanceChanges = [];

    [Inject] private IBalanceChangeService BalanceChangeService { get; set; } = null!;
    [Inject] private IFinancialOperationService FinancialOperationService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _unusedBalanceChanges = (await BalanceChangeService.GetUnusedAsync()).ToList();
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            var response = await FinancialOperationService.AddAsync(_newOperation);

            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("/financial-operations");
            }
            else
            {
                _errorMessage = "Failed to create new financial operation.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error creating financial operation: {ex.Message}";
        }
    }
}