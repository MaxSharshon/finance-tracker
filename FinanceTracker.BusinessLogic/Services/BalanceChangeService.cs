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
    public async Task<IEnumerable<BalanceChangeDto>> GetAllAsync() => 
        mapper.Map<IEnumerable<BalanceChangeDto>>(await unitOfWork.BalanceChanges.GetAllAsync());

    public async Task<BalanceChangeDto> GetById(Guid id) => 
        mapper.Map<BalanceChangeDto>(await GetEntityByIdAsync(id));

    public async Task<Guid> AddAsync(BalanceChangeDto balanceChangeDto)
    {
        var balanceChange = mapper.Map<BalanceChange>(balanceChangeDto);
        
        Validate(balanceChange);

        var isExists = (await unitOfWork.BalanceChanges.FindAsync(bc =>
            bc.OperationType == balanceChange.OperationType
            && bc.Amount == balanceChange.Amount)).Any();

        if (isExists)
        {
            throw new InvalidOperationException(
                $"A {nameof(BalanceChange)} with the same {nameof(BalanceChange.OperationType)} and " +
                $"{nameof(BalanceChange.Amount)} already exists.");
        }
        
        await unitOfWork.BalanceChanges.AddAsync(balanceChange);
        await unitOfWork.CompleteAsync();
        
        return balanceChange.Id;
    }

    public async Task UpdateAsync(BalanceChangeDto balanceChangeDto)
    {
        var existingBalanceChange = await GetEntityByIdAsync(balanceChangeDto.Id);
        mapper.Map(balanceChangeDto, existingBalanceChange);
        Validate(existingBalanceChange);
        unitOfWork.CompleteAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        unitOfWork.BalanceChanges.Remove(await GetEntityByIdAsync(id));
        unitOfWork.CompleteAsync();
    }

    private async Task<BalanceChange> GetEntityByIdAsync(Guid id)
    {
        return await unitOfWork.BalanceChanges.GetAsync(id) ??
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