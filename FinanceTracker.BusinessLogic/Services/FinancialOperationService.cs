using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Extensions;
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
    public async Task<IEnumerable<FinancialOperationDto>> GetAllAsync(Guid userId)
    {
        var operations = await unitOfWork.FinancialOperations.GetAllAsync(userId);
        return mapper.Map<IEnumerable<FinancialOperationDto>>(operations);
    }

    public async Task<IEnumerable<FinancialOperationDto>> GetAllAsync(Guid userId, FinancialOperationFilterDto filter)
    {
        var operations = await unitOfWork.FinancialOperations.GetAllAsync(
            userId,
            filter.StartDate,
            filter.EndDate,
            filter.CategoryId,
            filter.BudgetId,
            filter.OperationType);
        
        return mapper.Map<IEnumerable<FinancialOperationDto>>(operations);
    }

    public async Task<FinancialOperationDto> GetByIdAsync(Guid id, Guid userId)
    {
        return mapper.Map<FinancialOperationDto>(await GetEntityByIdAsync(id, userId));
    }

    public async Task<Guid> AddAsync(FinancialOperationDto operationDto, Guid userId)
    {
        await EnsureReferencesExistAsync(operationDto, userId);
        
        var operation = mapper.Map<FinancialOperation>(operationDto);
        operation.UserId = userId;
        
        SyncTags(operation, operationDto.TagIds);
        
        validator.EnsureValid(operation);
        
        await unitOfWork.FinancialOperations.AddAsync(operation);
        await unitOfWork.CompleteAsync();
        
        return operation.Id;
    }
    
    public async Task UpdateAsync(FinancialOperationDto operationDto, Guid userId)
    {
        await EnsureReferencesExistAsync(operationDto, userId);
        
        var existingOperation = await GetEntityByIdAsync(operationDto.Id, userId);
        
        mapper.Map(operationDto, existingOperation);
        existingOperation.UserId = userId;
        
        SyncTags(existingOperation, operationDto.TagIds);
        
        validator.EnsureValid(existingOperation);
        await unitOfWork.CompleteAsync();
    }

    public async Task RemoveAsync(Guid id, Guid userId)
    {
        unitOfWork.FinancialOperations.Remove(await GetEntityByIdAsync(id, userId));
        await unitOfWork.CompleteAsync();
    }

    private async Task<FinancialOperation> GetEntityByIdAsync(Guid id, Guid userId)
    {
        return await unitOfWork.FinancialOperations.GetByIdAsync(id, userId) 
               ?? throw new KeyNotFoundException($"A {nameof(FinancialOperation)} with ID {id} not found.");
    }

    private async Task EnsureReferencesExistAsync(FinancialOperationDto operationDto, Guid userId)
    {
        if (await unitOfWork.Categories.GetByIdAsync(operationDto.CategoryId, userId) is null)
        {
            throw new KeyNotFoundException($"A {nameof(Category)} with ID {operationDto.CategoryId} not found.");
        }

        if (operationDto.BudgetId.HasValue && 
            await unitOfWork.Budgets.GetByIdAsync(operationDto.BudgetId.Value, userId) is null)
        {
            throw new KeyNotFoundException($"A {nameof(Budget)} with ID {operationDto.BudgetId.Value} not found.");
        }
        
        foreach (var tagId in operationDto.TagIds.Distinct())
        {
            if (await unitOfWork.Tags.GetByIdAsync(tagId, userId) is null)
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
