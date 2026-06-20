using System.Globalization;
using FinanceTracker.Contracts.Budgets;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Budgets;

public partial class Listing
{
    private readonly BudgetFormModel _form = new();
    private readonly BudgetMemberFormModel _memberForm = new();
    private List<BudgetResponse> _budgets = [];
    private List<BudgetMemberResponse> _members = [];
    private Guid? _editingId;
    private Guid? _deletingId;
    private Guid? _removingMemberId;
    private bool _isLoading = true;
    private bool _isMembersLoading;
    private bool _isSaving;
    private string? _errorMessage;
    private string? _successMessage;

    [Inject] private IBudgetService BudgetService { get; set; } = null!;

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
            _budgets = (await BudgetService.GetAllAsync()).ToList();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load budgets: {ex.Message}";
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
        _form.LimitAmount = null;
        _form.StartDate = null;
        _form.EndDate = null;
        _memberForm.UserId = string.Empty;
        _members = [];
        _errorMessage = null;
        _successMessage = null;
    }

    private async Task StartEditAsync(BudgetResponse budget)
    {
        _editingId = budget.Id;
        _form.Name = budget.Name;
        _form.LimitAmount = budget.LimitAmount;
        _form.StartDate = budget.StartDate;
        _form.EndDate = budget.EndDate;
        _memberForm.UserId = string.Empty;
        _errorMessage = null;
        _successMessage = null;

        await LoadMembersAsync(budget.Id);
    }

    private async Task SaveAsync()
    {
        try
        {
            _isSaving = true;
            _errorMessage = null;
            _successMessage = null;

            var request = new BudgetRequest(
                _form.Name,
                _form.LimitAmount,
                _form.StartDate,
                _form.EndDate);

            var response = _editingId.HasValue
                ? await BudgetService.UpdateAsync(_editingId.Value, request)
                : await BudgetService.AddAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to save budget.";
                return;
            }

            var successMessage = _editingId.HasValue ? "Budget updated." : "Budget created.";
            StartCreate();
            _successMessage = successMessage;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to save budget: {ex.Message}";
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

            var response = await BudgetService.DeleteAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to delete budget.";
                return;
            }

            StartCreate();
            _successMessage = "Budget deleted.";
            await LoadAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to delete budget: {ex.Message}";
        }
        finally
        {
            _deletingId = null;
            _isSaving = false;
        }
    }

    private async Task LoadMembersAsync(Guid budgetId)
    {
        try
        {
            _isMembersLoading = true;
            _members = (await BudgetService.GetMembersAsync(budgetId)).ToList();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load budget members: {ex.Message}";
        }
        finally
        {
            _isMembersLoading = false;
        }
    }

    private async Task AddMemberAsync()
    {
        if (!_editingId.HasValue)
        {
            return;
        }

        if (!Guid.TryParse(_memberForm.UserId, out var memberUserId))
        {
            _errorMessage = "User id must be a valid GUID.";
            return;
        }

        try
        {
            _isSaving = true;
            _errorMessage = null;
            _successMessage = null;

            var response = await BudgetService.AddMemberAsync(
                _editingId.Value,
                new BudgetMemberRequest(memberUserId));

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to add budget member.";
                return;
            }

            _memberForm.UserId = string.Empty;
            _successMessage = "Budget member added.";
            await LoadMembersAsync(_editingId.Value);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to add budget member: {ex.Message}";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task RemoveMemberAsync(Guid memberUserId)
    {
        if (!_editingId.HasValue)
        {
            return;
        }

        try
        {
            _isSaving = true;
            _removingMemberId = memberUserId;
            _errorMessage = null;
            _successMessage = null;

            var response = await BudgetService.RemoveMemberAsync(_editingId.Value, memberUserId);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to remove budget member.";
                return;
            }

            _successMessage = "Budget member removed.";
            await LoadMembersAsync(_editingId.Value);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to remove budget member: {ex.Message}";
        }
        finally
        {
            _removingMemberId = null;
            _isSaving = false;
        }
    }

    private static string FormatMoney(decimal? amount) =>
        amount.HasValue ? amount.Value.ToString("C", CultureInfo.GetCultureInfo("uk-UA")) : "No limit";

    private static string FormatDate(DateTime? date) =>
        date.HasValue ? date.Value.ToShortDateString() : "-";

    private sealed class BudgetFormModel
    {
        public string Name { get; set; } = string.Empty;
        public decimal? LimitAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    private sealed class BudgetMemberFormModel
    {
        public string UserId { get; set; } = string.Empty;
    }
}
