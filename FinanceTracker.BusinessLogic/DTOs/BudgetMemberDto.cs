namespace FinanceTracker.BusinessLogic.DTOs;

public class BudgetMemberDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}