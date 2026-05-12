using FinanceTracker.Core.Enums;

namespace FinanceTracker.Core.Models;

public class FinancialOperation
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    public Guid? BudgetId { get; set; }
    public Budget? Budget { get; set; }
    public decimal Amount { get; set; }
    public OperationType OperationType { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid? BalanceChangeId { get; set; }
    public BalanceChange? BalanceChange { get; set; }

    public ICollection<OperationTag> OperationTags { get; set; } = [];
}
