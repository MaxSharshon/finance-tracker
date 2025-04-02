namespace FinanceTracker.UI.Models;

public class FinancialOperationRequest(DateTime date, Guid balanceChangeId)
{
    public DateTime Date { get; set; } = date;
    public Guid BalanceChangeId { get; set; } = balanceChangeId;

    public FinancialOperationRequest() : this(DateTime.Now, Guid.Empty)
    {
    }
}