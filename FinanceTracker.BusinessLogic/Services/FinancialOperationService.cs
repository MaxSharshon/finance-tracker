using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Services;

public class FinancialOperationService(
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    IValidator<FinancialOperation> validator)
    : IFinancialOperationService
{
    public async Task<IEnumerable<FinancialOperationDto>> GetAllAsync()
    {
        return mapper.Map<IEnumerable<FinancialOperationDto>>(await unitOfWork.FinancialOperations.GetAllAsync());
    }

    public async Task<FinancialOperationDto> GetByIdAsync(Guid id)
    {
        return mapper.Map<FinancialOperationDto>(await GetEntityByIdAsync(id));
    }

    public async Task<Guid> AddAsync(FinancialOperationDto operationDto)
    {
        await EnsureReferencesExistAsync(operationDto);
        
        var operation = mapper.Map<FinancialOperation>(operationDto);
        SyncTags(operation, operationDto.TagIds);
        
        Validate(operation);
        
        await unitOfWork.FinancialOperations.AddAsync(operation);
        await unitOfWork.CompleteAsync();
        
        return operation.Id;
    }
    
    public async Task UpdateAsync(FinancialOperationDto operationDto)
    {
        await EnsureReferencesExistAsync(operationDto);
        
        var existingOperation = await GetEntityByIdAsync(operationDto.Id);
        
        mapper.Map(operationDto, existingOperation);
        SyncTags(existingOperation, operationDto.TagIds);
        
        Validate(existingOperation);
        await unitOfWork.CompleteAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        unitOfWork.FinancialOperations.Remove(await GetEntityByIdAsync(id));
        await unitOfWork.CompleteAsync();
    }

    private async Task<FinancialOperation> GetEntityByIdAsync(Guid id)
    {
        return await unitOfWork.FinancialOperations.GetAsync(id) ??
               throw new KeyNotFoundException($"A {nameof(FinancialOperation)} with ID {id} not found.");
    }
    
    private void Validate(FinancialOperation operation)
    {
        var result = validator.Validate(operation);
        if (result.IsValid) return;
        
        var errors = string.Join(',', result.Errors.Select(e => e.ErrorMessage));
        throw new ValidationException($"Validation failed: {errors}");
    }

    private async Task EnsureReferencesExistAsync(FinancialOperationDto operationDto)
    {
        if (operationDto.CategoryId.HasValue && await unitOfWork.Categories.GetAsync(operationDto.CategoryId.Value) is null)
        {
            throw new KeyNotFoundException($"A {nameof(Category)} with ID {operationDto.CategoryId.Value} not found.");
        }

        if (operationDto.BudgetId.HasValue && await unitOfWork.Budgets.GetAsync(operationDto.BudgetId.Value) is null)
        {
            throw new KeyNotFoundException($"A {nameof(Budget)} with ID {operationDto.BudgetId.Value} not found.");
        }
        
        foreach (var tagId in operationDto.TagIds.Distinct())
        {
            if (await unitOfWork.Tags.GetAsync(tagId) is null)
            {
                throw new KeyNotFoundException($"A {nameof(Tag)} with ID {tagId} not found.");
            }
        }
    }

    private static void SyncTags(FinancialOperation operation, IEnumerable<Guid> tagIds)
    {
        operation.OperationTags.Clear();

        foreach (var tagId in tagIds.Distinct())
        {
            operation.OperationTags.Add(new OperationTag
            {
                FinancialOperationId = operation.Id,
                TagId = tagId
            });
        }
    }
}
