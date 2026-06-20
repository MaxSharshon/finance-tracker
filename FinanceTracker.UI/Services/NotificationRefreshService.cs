using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class NotificationRefreshService : INotificationRefreshService
{
    public event Func<Task>? RefreshRequested;

    public async Task RequestRefreshAsync()
    {
        if (RefreshRequested is not null)
        {
            await RefreshRequested.Invoke();
        }
    }
}
