using FinanceTracker.Contracts.Notifications;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace FinanceTracker.UI.Components.Layout;

public partial class MainLayout : IDisposable
{
    [Inject] private IAuthClientService AuthClient { get; set; } = null!;
    [Inject] private INotificationClientService NotificationService { get; set; } = null!;
    [Inject] private INotificationRefreshService NotificationRefreshService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private List<NotificationResponse> _notifications = [];

    private string DisplayUserEmail => AuthClient.UserEmail ?? "Signed in";
    private string DisplayUserName => AuthClient.DisplayName ?? AuthClient.UserEmail ?? "Account";
    private string UserInitials => GetUserInitials(AuthClient.DisplayName ?? AuthClient.UserEmail);
    private int UnreadNotificationsCount => _notifications.Count(notification => !notification.IsRead);
    private IEnumerable<NotificationResponse> HeaderNotifications => _notifications
        .Where(notification => !notification.IsRead)
        .OrderByDescending(notification => notification.CreatedAt)
        .Take(3);

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += HandleLocationChanged;
        NotificationRefreshService.RefreshRequested += RefreshNotificationsAsync;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        RedirectIfUnauthorized();
        await RefreshNotificationsAsync();
    }

    private async void HandleLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        RedirectIfUnauthorized();
        await RefreshNotificationsAsync();
    }

    private async Task RefreshNotificationsAsync()
    {
        if (!AuthClient.IsAuthenticated)
        {
            _notifications = [];
            await InvokeAsync(StateHasChanged);
            return;
        }

        try
        {
            _notifications = (await NotificationService.GetAllAsync()).ToList();
        }
        catch
        {
            _notifications = [];
        }

        await InvokeAsync(StateHasChanged);
    }

    private void RedirectIfUnauthorized()
    {
        var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        var path = relativePath.Split('?', '#')[0].Trim('/');
        var isPublicPage =
            string.Equals(path, "login", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(path, "register", StringComparison.OrdinalIgnoreCase);

        switch (AuthClient.IsAuthenticated)
        {
            case false when !isPublicPage:
                NavigationManager.NavigateTo("/login");
                return;
            case true when isPublicPage:
                NavigationManager.NavigateTo("/");
                break;
        }
    }

    private void SignOut()
    {
        AuthClient.SignOut();
        _notifications = [];
        NavigationManager.NavigateTo("/login");
    }

    private static string GetUserInitials(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return "U";
        }

        var namePart = email.Contains('@') ? email.Split('@')[0] : email;
        var parts = namePart.Split([' ', '.', '_', '-'], StringSplitOptions.RemoveEmptyEntries);

        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[1][0]}".ToUpperInvariant()
            : namePart[..Math.Min(namePart.Length, 2)].ToUpperInvariant();
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= HandleLocationChanged;
        NotificationRefreshService.RefreshRequested -= RefreshNotificationsAsync;
    }
}
