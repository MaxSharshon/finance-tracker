using FinanceTracker.Core.Enums;

namespace FinanceTracker.BusinessLogic.DTOs;

public class CategorySuggestionDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public OperationType OperationType { get; set; }
    public decimal Confidence { get; set; }
    public string? MatchedKeyword { get; set; }
}
