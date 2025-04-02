namespace FinanceTracker.UI.Models;

public class BalanceChangeRequest(decimal amount, string operationType)
{
    public decimal Amount { get; set; } = amount;
    public string OperationType { get; set; } = operationType;

    public BalanceChangeRequest() : this(0, string.Empty)
    {
    }
}