using FinanceTracker.Contracts.Auth;

namespace FinanceTracker.UI.Services.Interfaces;

public interface IAuthClientService
{
    bool IsAuthenticated { get; }
    string? UserEmail { get; }
    string? DisplayName { get; }

    Task<bool> LoginAsync(LoginRequest request);
    Task<HttpResponseMessage> RegisterAsync(RegisterRequest request);
    void SignOut();
}
