namespace FinanceTracker.BusinessLogic.DTOs;

public class FinancialOperationDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public Guid BalanceChangeId { get; set; }
}