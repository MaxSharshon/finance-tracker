using FinanceTracker.Core.Models;

namespace FinanceTracker.DataAccess.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
