namespace FinanceTracker.UI.Services.Interfaces;

public interface INotificationRefreshService
{
    event Func<Task>? RefreshRequested;
    Task RequestRefreshAsync();
}
