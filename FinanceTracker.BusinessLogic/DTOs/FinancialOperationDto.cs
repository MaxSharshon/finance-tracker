using FinanceTracker.Core.Enums;

namespace FinanceTracker.BusinessLogic.DTOs;

public class FinancialOperationDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? BudgetId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<Guid> TagIds { get; set; } = [];
}
