using System.Net.Http.Headers;
using FinanceTracker.Contracts.Auth;
using FinanceTracker.UI.Services.Interfaces;

namespace FinanceTracker.UI.Services;

public class AuthClientService(HttpClient client) : IAuthClientService
{
    private string? _token;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_token);
    public string? UserEmail { get; private set; }
    public string? DisplayName { get; private set; }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        var response = await client.PostAsJsonAsync("Auth/login", request);

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (auth is null || string.IsNullOrWhiteSpace(auth.Token))
        {
            return false;
        }

        _token = auth.Token;
        UserEmail = auth.Email;
        DisplayName = auth.DisplayName;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        return true;
    }

    public async Task<HttpResponseMessage> RegisterAsync(RegisterRequest request)
    {
        return await client.PostAsJsonAsync("Auth/register", request);
    }

    public void SignOut()
    {
        _token = null;
        UserEmail = null;
        DisplayName = null;
        client.DefaultRequestHeaders.Authorization = null;
    }
}
