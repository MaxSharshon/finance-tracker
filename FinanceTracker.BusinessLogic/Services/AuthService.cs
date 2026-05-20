using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;

namespace FinanceTracker.BusinessLogic.Services;

public class AuthService(IUnitOfWork unitOfWork) : IAuthService
{
    public async Task<Guid> RegisterAsync(RegisterDto registerDto)
    {
        if (await unitOfWork.Users.GetByEmailAsync(registerDto.Email) is not null)
        {
            throw new InvalidOperationException($"User with email {registerDto.Email} already exists.");
        }

        var user = new User
        {
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            DisplayName = registerDto.DisplayName
        };
        
        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.CompleteAsync();
        
        return user.Id;
    }

    public async Task<User?> ValidateCredentialsAsync(LoginDto loginDto)
    {
        var user = await unitOfWork.Users.GetByEmailAsync(loginDto.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return null;
        }
        
        return user;
    }
}