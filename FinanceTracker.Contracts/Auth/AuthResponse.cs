namespace FinanceTracker.Contracts.Auth;

public record AuthResponse(string Token, string Email, string DisplayName);
