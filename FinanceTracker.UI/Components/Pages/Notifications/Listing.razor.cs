using FinanceTracker.Contracts.Notifications;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Notifications;

public partial class Listing
{
    private List<NotificationResponse> _notifications = [];
    private bool _isLoading = true;
    private bool _isSaving;
    private string? _errorMessage;

    [Inject] private INotificationClientService NotificationService { get; set; } = null!;
    [Inject] private INotificationRefreshService NotificationRefreshService { get; set; } = null!;

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
            _notifications = (await NotificationService.GetAllAsync()).ToList();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to load notifications: {ex.Message}";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task MarkAllAsReadAsync()
    {
        try
        {
            _isSaving = true;
            _errorMessage = null;

            foreach (var notification in _notifications.Where(notification => !notification.IsRead))
            {
                await NotificationService.UpdateAsync(
                    notification.Id,
                    new NotificationRequest(notification.Message, true));
            }

            await LoadAsync();
            await NotificationRefreshService.RequestRefreshAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to update notifications: {ex.Message}";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task ToggleRead(NotificationResponse notification)
    {
        try
        {
            _isSaving = true;
            _errorMessage = null;

            var response = await NotificationService.UpdateAsync(
                notification.Id,
                new NotificationRequest(notification.Message, !notification.IsRead));

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to update notification.";
                return;
            }

            await LoadAsync();
            await NotificationRefreshService.RequestRefreshAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to update notification: {ex.Message}";
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
            _errorMessage = null;

            var response = await NotificationService.DeleteAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                _errorMessage = "Failed to delete notification.";
                return;
            }

            await LoadAsync();
            await NotificationRefreshService.RequestRefreshAsync();
        }
        catch (Exception ex)
        {
            _errorMessage = $"Failed to delete notification: {ex.Message}";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private static string GetNotificationSource(NotificationResponse notification)
    {
        if (notification.Message.Contains("budget", StringComparison.OrdinalIgnoreCase))
        {
            return "Budget";
        }

        if (notification.Message.Contains("Imported", StringComparison.OrdinalIgnoreCase))
        {
            return "Import";
        }

        return "System";
    }
}
