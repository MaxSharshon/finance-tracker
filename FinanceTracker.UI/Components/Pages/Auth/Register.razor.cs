using FinanceTracker.Contracts.Auth;
using FinanceTracker.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FinanceTracker.UI.Components.Pages.Auth;

public partial class Register
{
    private readonly RegisterFormModel _form = new();

    private string? _message;
    private string _messageClass = "validation-message";

    [Inject] private IAuthClientService AuthClient { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private async Task HandleRegisterAsync()
    {
        var response = await AuthClient.RegisterAsync(new RegisterRequest(
            _form.Email,
            _form.Password,
            _form.DisplayName));

        if (response.IsSuccessStatusCode)
        {
            _messageClass = "stat-note";
            _message = "Account created. You can sign in now.";
            NavigationManager.NavigateTo("login");
            return;
        }

        _messageClass = "validation-message";
        _message = "Failed to create account. Check entered data.";
    }

    private sealed class RegisterFormModel
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
