using FinanceTracker.API.Services;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Contracts.Auth;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    IAuthService authService,
    JwtProvider jwtProvider,
    IValidator<RegisterRequest> registerValidator,
    IValidator<LoginRequest> loginValidator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        var validationResult = await registerValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ToValidationErrors(validationResult)));
        }

        var id = await authService.RegisterAsync(new RegisterDto
        {
            Email = request.Email,
            Password = request.Password,
            DisplayName = request.DisplayName
        });

        return Ok(new { id });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> LoginAsync([FromBody] LoginRequest request)
    {
        var validationResult = await loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ToValidationErrors(validationResult)));
        }

        var user = await authService.ValidateCredentialsAsync(new LoginDto
        {
            Email = request.Email,
            Password = request.Password
        });

        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(new AuthResponse(jwtProvider.Create(user)));
    }

    private static Dictionary<string, string[]> ToValidationErrors(ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());
    }
}
