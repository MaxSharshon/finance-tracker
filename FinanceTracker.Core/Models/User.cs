using FinanceTracker.Core.Enums;

namespace FinanceTracker.Core.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;

    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<FinancialOperation> FinancialOperations { get; set; } = [];
    public ICollection<BudgetUser> BudgetUsers { get; set; } = [];
    public ICollection<Budget> OwnedBudgets { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
}
