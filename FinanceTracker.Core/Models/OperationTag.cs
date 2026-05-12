namespace FinanceTracker.Core.Models;

public class OperationTag
{
    public Guid FinancialOperationId { get; set; }
    public FinancialOperation? FinancialOperation { get; set; }
    public Guid TagId { get; set; }
    public Tag? Tag { get; set; }
}
