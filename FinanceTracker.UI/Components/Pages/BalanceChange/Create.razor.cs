using FinanceTracker.UI.Enums;
using FinanceTracker.UI.Models;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.BalanceChange;

public partial class Create
{
    private BalanceChangeRequest _newBalanceChange = new();
    private string? _errorMessage;
    private readonly List<string> _operationTypeOptions = Enum.GetNames<OperationType>().ToList();

    [Inject] private IBalanceChangeService Service { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private async Task HandleValidSubmit()
    {
        try
        {
            var response = await Service.AddAsync(_newBalanceChange);

            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("/balance-changes");
            }
            else
            {
                _errorMessage = "Failed to create new balance change.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error creating balance change: {ex.Message}";
        }
    }
}