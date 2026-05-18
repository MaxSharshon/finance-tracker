using FinanceTracker.Core.Enums;

namespace FinanceTracker.Core.Models;

public class Category
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Name { get; set; } = string.Empty;
    public OperationType OperationType { get; set; }

    public ICollection<FinancialOperation> FinancialOperations { get; set; } = [];
}
