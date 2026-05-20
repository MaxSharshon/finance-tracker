using System.Security.Claims;
using System.Text;
using FinanceTracker.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FinanceTracker.API.Services;

public class JwtProvider(IOptions<JwtOptions> options)
{
    private readonly JwtOptions _jwtOptions = options.Value;
    
    public string Create(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}