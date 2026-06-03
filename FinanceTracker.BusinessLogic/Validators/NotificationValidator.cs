using FinanceTracker.Core.Models;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Validators;

public class NotificationValidator : AbstractValidator<Notification>
{
    public NotificationValidator()
    {
        RuleFor(notification => notification.UserId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
        
        RuleFor(notification => notification.Message)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(512).WithMessage("{PropertyName} is too long.");
        
        RuleFor(notification => notification.CreatedAt)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}
