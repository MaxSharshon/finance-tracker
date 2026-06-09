using FinanceTracker.Core.Enums;

namespace FinanceTracker.BusinessLogic.DTOs.Reports;

public class CategoryReportSummaryDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public OperationType OperationType { get; set; }
    public decimal TotalAmount { get; set; }
    public int OperationsCount { get; set; }
}