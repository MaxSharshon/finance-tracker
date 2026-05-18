using FluentValidation;

namespace FinanceTracker.BusinessLogic.Extensions;

public static class ValidatorExtensions
{
    public static void EnsureValid<T>(this IValidator<T> validator, T entity)
    {
        var result = validator.Validate(entity);
        if (result.IsValid)
        {
            return;
        }
        
        var errors = string.Join(',', result.Errors.Select(e => e.ErrorMessage));
        throw new ValidationException($"Validation failed: {errors}");
    }
}