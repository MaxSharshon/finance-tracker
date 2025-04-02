using AutoMapper;
using FinanceTracker.UI.Enums;
using FinanceTracker.UI.Models;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.BalanceChange;

public partial class Edit
{
    [Parameter] public string Id { get; set; } = string.Empty;
    
    private Guid BalanceChangeId => Guid.TryParse(Id, out var id) ? id : Guid.Empty;
    private BalanceChangeRequest? _balanceChange;
    private string? _errorMessage;
    private readonly List<string> _operationTypeOptions = Enum.GetNames<OperationType>().ToList();

    [Inject] private IBalanceChangeService Service { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IMapper Mapper { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (BalanceChangeId == Guid.Empty) 
        {
            _errorMessage ="Invalid Balance Change ID";
            return;
        }
        try
        {
            var response = await Service.GetAsync(BalanceChangeId);
            if (response is not null)
            {
                _balanceChange = Mapper.Map<BalanceChangeRequest>(response);
            }
            else
            {
                _errorMessage = "Balance change not found.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error loading balance change: {ex.Message}";
        }
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            var response = await Service.UpdateAsync(BalanceChangeId, _balanceChange);

            if (response.IsSuccessStatusCode)
            {
                GoToListing();
            }
            else
            {
                _errorMessage = "Failed to update balance change.";
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error updating balance change: {ex.Message}";
        }
    }

    private void GoToListing()
    {
        NavigationManager.NavigateTo("/balance-changes");
    }
}