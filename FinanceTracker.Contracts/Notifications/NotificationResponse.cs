namespace FinanceTracker.Contracts.Notifications;

public record NotificationResponse(Guid Id, string Message, bool IsRead, DateTime CreatedAt);
