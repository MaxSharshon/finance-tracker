using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Extensions;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Services;

public class BudgetService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<Budget> validator) 
    : IBudgetService
{
    public async Task<IEnumerable<BudgetDto>> GetAllAsync(Guid userId)
    {
        var budgets = await unitOfWork.Budgets.GetByUserAsync(userId);
        return mapper.Map<IEnumerable<BudgetDto>>(budgets);
    }
    
    public async Task<IEnumerable<BudgetMemberDto>> GetMembersAsync(Guid budgetId, Guid userId)
    {
        var budget = await GetEntityByIdAsync(budgetId, userId);
        return mapper.Map<IEnumerable<BudgetMemberDto>>(budget.BudgetUsers);
    }

    public async Task AddMemberAsync(Guid budgetId, Guid memberUserId, Guid userId)
    {
        var budget = await GetEntityByIdAsync(budgetId, userId);
        EnsureOwner(budget, userId);

        if (memberUserId == budget.OwnerUserId)
        {
            throw new InvalidOperationException("Budget owner is already a member.");
        }

        if (await unitOfWork.Users.GetAsync(memberUserId) is null)
        {
            throw new KeyNotFoundException($"A {nameof(User)} with ID {memberUserId} not found.");
        }

        if (budget.BudgetUsers.Any(member => member.UserId == memberUserId))
        {
            throw new InvalidOperationException("User is already a member of this budget.");
        }
        
        budget.BudgetUsers.Add(new BudgetUser
        {
            BudgetId = budgetId,
            UserId = memberUserId
        });
        
        await unitOfWork.CompleteAsync();
    }

    public async Task RemoveMemberAsync(Guid budgetId, Guid memberUserId, Guid userId)
    {
        var budget = await GetEntityByIdAsync(budgetId, userId);
        EnsureOwner(budget, userId);
        
        var member = budget.BudgetUsers.FirstOrDefault(member => member.UserId == memberUserId)
            ?? throw new KeyNotFoundException($"A {nameof(BudgetUser)} with ID {memberUserId} not found.");
        
        budget.BudgetUsers.Remove(member);
        await unitOfWork.CompleteAsync();
    }

    public async Task<BudgetDto> GetByIdAsync(Guid id, Guid userId)
    {
        return mapper.Map<BudgetDto>(await GetEntityByIdAsync(id, userId));
    }

    public async Task<Guid> AddAsync(BudgetDto budgetDto, Guid userId)
    {
        var budget = mapper.Map<Budget>(budgetDto);
        budget.OwnerUserId = userId;
        
        validator.EnsureValid(budget);
        
        await unitOfWork.Budgets.AddAsync(budget);
        await unitOfWork.CompleteAsync();

        return budget.Id;
    }

    public async Task UpdateAsync(BudgetDto budgetDto, Guid userId)
    {
        var existingBudget = await GetEntityByIdAsync(budgetDto.Id, userId);
        EnsureOwner(existingBudget, userId);
        
        mapper.Map(budgetDto, existingBudget);
        existingBudget.OwnerUserId = userId;
        
        validator.EnsureValid(existingBudget);
        
        await unitOfWork.CompleteAsync();
    }

    public async Task RemoveAsync(Guid id, Guid userId)
    {
        var budget = await GetEntityByIdAsync(id, userId);
        
        EnsureOwner(budget, userId);
        
        unitOfWork.Budgets.Remove(budget);
        await unitOfWork.CompleteAsync();
    }

    private async Task<Budget> GetEntityByIdAsync(Guid id, Guid userId)
    {
        return await unitOfWork.Budgets.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException($"A {nameof(Budget)} with ID {id} not found.");
    }

    private static void EnsureOwner(Budget budget, Guid userId)
    {
        if (budget.OwnerUserId != userId)
        {
            throw new UnauthorizedAccessException("Only the owner can modify this budget.");
        }   
    }
}
