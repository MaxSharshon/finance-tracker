namespace FinanceTracker.Core.Models;

public class Tag
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<OperationTag> OperationTags { get; set; } = [];
}
