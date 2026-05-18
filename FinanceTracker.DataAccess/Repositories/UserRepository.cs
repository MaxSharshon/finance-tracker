using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class UserRepository(FinanceTrackerContext context) : Repository<User>(context), IUserRepository
{
    private FinanceTrackerContext FinanceTrackerContext => (FinanceTrackerContext)Context;

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await FinanceTrackerContext.Users
            .FirstOrDefaultAsync(user => user.Email == email);
    }
}
