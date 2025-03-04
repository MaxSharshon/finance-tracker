namespace FinanceTracker.Core.Models;

public class FinancialOperation
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public Guid BalanceChangeId { get; set; }
    public BalanceChange BalanceChange { get; set; }
}