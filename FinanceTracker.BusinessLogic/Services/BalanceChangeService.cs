using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;

namespace FinanceTracker.BusinessLogic.Services;

public class BalanceChangeService(IUnitOfWork unitOfWork) : IBalanceChangeService
{
    public IEnumerable<BalanceChange> GetAll() => unitOfWork.BalanceChanges.GetAll();

    public BalanceChange GetById(Guid id)
    {
        return unitOfWork.BalanceChanges.Get(id)
               ?? throw new KeyNotFoundException($"{nameof(BalanceChange)} with ID {id} not found.");
    }

    public void Add(BalanceChange balanceChange)
    {
        var exists = unitOfWork.BalanceChanges.Find(bc =>
            bc.OperationType == balanceChange.OperationType 
            && bc.Amount == balanceChange.Amount).Any();

        if (exists)
        {
            throw new InvalidOperationException(
                $"A {nameof(BalanceChange)} with the same {nameof(BalanceChange.OperationType)} and " +
                $"{nameof(BalanceChange.Amount)} already exists.");
        }
        
        unitOfWork.BalanceChanges.Add(balanceChange);
        unitOfWork.Complete();
    }

    public void Update(BalanceChange balanceChange)
    {
        var existing = GetById(balanceChange.Id);
        existing.OperationType = balanceChange.OperationType;
        existing.Amount = balanceChange.Amount;

        unitOfWork.Complete();
    }

    public void Remove(Guid id)
    {
        unitOfWork.BalanceChanges.Remove(GetById(id));
        unitOfWork.Complete();
    }
}