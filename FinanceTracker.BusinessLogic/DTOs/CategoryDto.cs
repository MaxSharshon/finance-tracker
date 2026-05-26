using FinanceTracker.Core.Enums;

namespace FinanceTracker.BusinessLogic.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public OperationType OperationType { get; set; }
}
