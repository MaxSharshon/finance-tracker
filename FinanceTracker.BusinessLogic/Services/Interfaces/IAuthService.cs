using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.Core.Models;

namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IAuthService
{
    Task<Guid> RegisterAsync(RegisterDto registerDto);
    Task<User?> ValidateCredentialsAsync(LoginDto loginDto);
}