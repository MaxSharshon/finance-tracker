using System.Security.Claims;

namespace FinanceTracker.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var rawUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(rawUserId) || !Guid.TryParse(rawUserId, out var userId))
        {
            throw new UnauthorizedAccessException("User id claim is missing or invalid.");
        }
        
        return userId;
    }
}