namespace FinanceTracker.Contracts.Budgets;

public record BudgetMemberResponse(Guid UserId, string Email, string DisplayName);
