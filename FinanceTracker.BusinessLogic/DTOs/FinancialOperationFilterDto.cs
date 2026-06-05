using FinanceTracker.Core.Enums;

namespace FinanceTracker.BusinessLogic.DTOs;

public class FinancialOperationFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? BudgetId { get; set; }
    public OperationType? OperationType { get; set; }
}
