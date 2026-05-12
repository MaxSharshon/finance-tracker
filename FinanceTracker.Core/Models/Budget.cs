namespace FinanceTracker.Core.Models;

public class Budget
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid OwnerUserId { get; set; }
    public User? OwnerUser { get; set; }
    public decimal? LimitAmount { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ICollection<BudgetUser> BudgetUsers { get; set; } = [];
    public ICollection<FinancialOperation> FinancialOperations { get; set; } = [];
}
