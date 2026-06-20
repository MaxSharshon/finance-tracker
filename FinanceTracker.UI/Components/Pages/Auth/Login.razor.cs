using FinanceTracker.Contracts.Auth;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Auth;

public partial class Login
{
    private readonly LoginFormModel _form = new();

    private string? _errorMessage;

    [Inject] private IAuthClientService AuthClient { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private async Task HandleLoginAsync()
    {
        _errorMessage = null;

        var success = await AuthClient.LoginAsync(new LoginRequest(_form.Email, _form.Password));

        if (success)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        _errorMessage = "Invalid email or password.";
    }

    private sealed class LoginFormModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
