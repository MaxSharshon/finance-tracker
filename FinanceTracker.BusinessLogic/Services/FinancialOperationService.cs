using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Services;

public class FinancialOperationService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<FinancialOperation> validator)
    : IFinancialOperationService
{
    public async Task<IEnumerable<FinancialOperationDto>> GetAllAsync() => 
        mapper.Map<IEnumerable<FinancialOperationDto>>(await unitOfWork.FinancialOperations.GetAllAsync());
    
    public async Task<FinancialOperationDto> GetById(Guid id) => 
        mapper.Map<FinancialOperationDto>(await GetEntityByIdAsync(id));
    
    public async Task<Guid> AddAsync(FinancialOperationDto financialOperationDto)
    {
        await CheckForConflicts(financialOperationDto);
        
        var financialOperation = mapper.Map<FinancialOperation>(financialOperationDto);
        Validate(financialOperation);
        
        await unitOfWork.FinancialOperations.AddAsync(financialOperation);
        await unitOfWork.CompleteAsync();
        
        return financialOperation.Id;
    }
    
    public async Task UpdateAsync(FinancialOperationDto financialOperationDto)
    {
        await CheckForConflicts(financialOperationDto);
        var existingFinancialOperation = await GetEntityByIdAsync(financialOperationDto.Id);
        mapper.Map(financialOperationDto, existingFinancialOperation);
        Validate(existingFinancialOperation);
        unitOfWork.CompleteAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        unitOfWork.FinancialOperations.Remove(await GetEntityByIdAsync(id));
        unitOfWork.CompleteAsync();
    }

    private async Task<FinancialOperation> GetEntityByIdAsync(Guid id)
    {
        return await unitOfWork.FinancialOperations.GetAsync(id) ??
               throw new KeyNotFoundException($"{nameof(FinancialOperation)} with ID {id} not found.");
    }
    
    private void Validate(FinancialOperation financialOperation)
    {
        var result = validator.Validate(financialOperation);
        if (result.IsValid) return;
        
        var errors = string.Join(',', result.Errors.Select(e => e.ErrorMessage));
        throw new ValidationException($"Validation failed: {errors}");
    }
    
    private async Task EnsureBalanceChangeExistsAsync(Guid balanceChangeId)
    {
        if (await unitOfWork.BalanceChanges.GetAsync(balanceChangeId) == null)
        {
            throw new KeyNotFoundException($"A {nameof(BalanceChange)} with ID {balanceChangeId} doesn't exist.");
        }
    }

    private async Task EnsureBalanceChangeHaveNotAlreadyTakenAsync(Guid balanceChangeId, Guid? excludedId = null)
    {
        var isExists = (await unitOfWork.FinancialOperations.FindAsync(fo => 
            fo.BalanceChangeId == balanceChangeId 
            && (excludedId == null || fo.Id != excludedId))).Any();
        
        if (isExists)
        {
            throw new InvalidOperationException(
                $"A {nameof(FinancialOperation)} with {nameof(FinancialOperation.BalanceChangeId)} {balanceChangeId} already exists.");
        }
    }

    private async Task CheckForConflicts(FinancialOperationDto financialOperationDto)
    {
        await EnsureBalanceChangeExistsAsync(financialOperationDto.BalanceChangeId);
        await EnsureBalanceChangeHaveNotAlreadyTakenAsync(financialOperationDto.BalanceChangeId, financialOperationDto.Id);
    }
}