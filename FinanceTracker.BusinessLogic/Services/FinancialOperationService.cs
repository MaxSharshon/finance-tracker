using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Extensions;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Enums;
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
        var category = await GetCategoryByIdAsync(operationDto.CategoryId, userId);
        var budgetAlert = await BuildBudgetLimitNotificationAsync(operationDto, category, userId);
        
        var operation = mapper.Map<FinancialOperation>(operationDto);
        operation.UserId = userId;
        
        SyncTags(operation, operationDto.TagIds);
        
        validator.EnsureValid(operation);
        
        await unitOfWork.FinancialOperations.AddAsync(operation);
        
        if (budgetAlert is not null)
        {
            await unitOfWork.Notifications.AddAsync(budgetAlert);
        }
        
        await unitOfWork.CompleteAsync();
        
        return operation.Id;
    }
    
    public async Task UpdateAsync(FinancialOperationDto operationDto, Guid userId)
    {
        await EnsureReferencesExistAsync(operationDto, userId);
        
        var existingOperation = await GetEntityByIdAsync(operationDto.Id, userId);
        var category = await GetCategoryByIdAsync(operationDto.CategoryId, userId);
        var budgetAlert = await BuildBudgetLimitNotificationAsync(
            operationDto,
            category,
            userId,
            existingOperation.Id);
        
        mapper.Map(operationDto, existingOperation);
        existingOperation.UserId = userId;
        
        SyncTags(existingOperation, operationDto.TagIds);
        
        validator.EnsureValid(existingOperation);

        if (budgetAlert is not null)
        {
            await unitOfWork.Notifications.AddAsync(budgetAlert);
        }

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
        await GetCategoryByIdAsync(operationDto.CategoryId, userId);

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

    private async Task<Category> GetCategoryByIdAsync(Guid categoryId, Guid userId)
    {
        return await unitOfWork.Categories.GetByIdAsync(categoryId, userId)
               ?? throw new KeyNotFoundException($"A {nameof(Category)} with ID {categoryId} not found.");
    }

    private async Task<Notification?> BuildBudgetLimitNotificationAsync(
        FinancialOperationDto operationDto,
        Category category,
        Guid userId,
        Guid? excludedOperationId = null)
    {
        if (!ShouldCheckBudgetLimit(operationDto, category))
        {
            return null;
        }

        var budget = await GetBudgetWithLimitAsync(operationDto.BudgetId!.Value, userId);

        if (budget is null)
        {
            return null;
        }

        var previousSpent = await CalculatePreviousBudgetExpensesAsync(
            budget,
            userId,
            excludedOperationId);

        var currentSpent = previousSpent + operationDto.Amount;
        var message = BuildBudgetLimitMessage(budget, previousSpent, currentSpent);

        return message is null
            ? null
            : CreateBudgetLimitNotification(userId, message);
    }
    
    private static bool ShouldCheckBudgetLimit(
        FinancialOperationDto operationDto,
        Category category)
    {
        return operationDto.BudgetId.HasValue &&
               category.OperationType == OperationType.Expense;
    }
    
    private async Task<Budget?> GetBudgetWithLimitAsync(Guid budgetId, Guid userId)
    {
        var budget = await unitOfWork.Budgets.GetByIdAsync(budgetId, userId);

        return budget?.LimitAmount is > 0
            ? budget
            : null;
    }
    
    private async Task<decimal> CalculatePreviousBudgetExpensesAsync(
        Budget budget,
        Guid userId,
        Guid? excludedOperationId)
    {
        var expenses = (await unitOfWork.FinancialOperations.GetAllAsync(
                userId,
                budget.StartDate,
                budget.EndDate,
                null,
                budget.Id,
                OperationType.Expense))
            .ToList();

        return excludedOperationId.HasValue
            ? expenses
                .Where(operation => operation.Id != excludedOperationId.Value)
                .Sum(operation => operation.Amount)
            : expenses.Sum(operation => operation.Amount);
    }
    private static string? BuildBudgetLimitMessage(
        Budget budget,
        decimal previousSpent,
        decimal currentSpent)
    {
        var limit = budget.LimitAmount!.Value;
        var previousRatio = previousSpent / limit;
        var currentRatio = currentSpent / limit;

        return currentRatio switch
        {
            >= 1.0m =>
                $"Budget '{budget.Name}' limit exceeded. Current spending is {currentSpent:C} of {limit:C}.",
            >= 0.8m when previousRatio < 0.8m =>
                $"Budget '{budget.Name}' reached 80% of the limit. Current spending is {currentSpent:C} of {limit:C}.",
            _ => null
        };
    }
    private static Notification CreateBudgetLimitNotification(Guid userId, string message)
    {
        return new Notification
        {
            UserId = userId,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
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
