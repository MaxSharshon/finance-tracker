using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Services;

public class BalanceChangeService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<BalanceChange> validator)
    : IBalanceChangeService
{
    public IEnumerable<BalanceChangeDto> GetAll() => 
        mapper.Map<IEnumerable<BalanceChangeDto>>(unitOfWork.BalanceChanges.GetAll());

    public BalanceChangeDto GetById(Guid id) => 
        mapper.Map<BalanceChangeDto>(GetEntityById(id));

    public Guid Add(BalanceChangeDto balanceChangeDto)
    {
        var balanceChange = mapper.Map<BalanceChange>(balanceChangeDto);
        
        Validate(balanceChange);
        
        var isExists = unitOfWork.BalanceChanges.Find(bc =>
            bc.OperationType == balanceChange.OperationType 
            && bc.Amount == balanceChange.Amount).Any();

        if (isExists)
        {
            throw new InvalidOperationException(
                $"A {nameof(BalanceChange)} with the same {nameof(BalanceChange.OperationType)} and " +
                $"{nameof(BalanceChange.Amount)} already exists.");
        }
        
        unitOfWork.BalanceChanges.Add(balanceChange);
        unitOfWork.Complete();
        
        return balanceChange.Id;
    }

    public void Update(BalanceChangeDto balanceChangeDto)
    {
        var existingBalanceChange = GetEntityById(balanceChangeDto.Id);
        mapper.Map(balanceChangeDto, existingBalanceChange);
        Validate(existingBalanceChange);
        unitOfWork.Complete();
    }

    public void Remove(Guid id)
    {
        unitOfWork.BalanceChanges.Remove(GetEntityById(id));
        unitOfWork.Complete();
    }

    private BalanceChange GetEntityById(Guid id)
    {
        return unitOfWork.BalanceChanges.Get(id) ??
               throw new KeyNotFoundException($"{nameof(BalanceChange)} with ID {id} not found.");
    }

    private void Validate(BalanceChange balanceChange)
    {
        var result = validator.Validate(balanceChange);
        if (result.IsValid) return;
        
        var errors = string.Join(',', result.Errors.Select(e => e.ErrorMessage));
        throw new ValidationException($"Validation failed: {errors}");
    }
}