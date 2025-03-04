using FinanceTracker.Core.Enums;

namespace FinanceTracker.Core.Models;

public class BalanceChange
{
    public Guid Id { get; set; }
    public OperationType OperationType { get; set; }
    public decimal Amount { get; set; }
}