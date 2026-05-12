namespace FinanceTracker.Core.Models;

public class BudgetUser
{
    public Guid BudgetId { get; set; }
    public Budget? Budget { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
