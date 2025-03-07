using FinanceTracker.Core.Enums;

namespace FinanceTracker.BusinessLogic.DTOs;

public class BalanceChangeDto
{
    public Guid Id { get; set; }
    public OperationType OperationType { get; set; }
    public decimal Amount { get; set; }
}