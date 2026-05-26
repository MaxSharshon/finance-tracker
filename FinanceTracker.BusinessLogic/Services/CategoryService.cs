using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Extensions;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Services;

public class CategoryService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<Category> validator) 
    : ICategoryService
{
    public async Task<IEnumerable<CategoryDto>> GetAllAsync(Guid userId)
    {
        var categories = await unitOfWork.Categories.GetByUserAsync(userId);
        return mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
    
    public async Task<CategoryDto> GetByIdAsync(Guid id, Guid userId)
    {
        return mapper.Map<CategoryDto>(await GetEntityByIdAsync(id, userId));
    }

    public async Task<Guid> AddAsync(CategoryDto categoryDto, Guid userId)
    {
        var category = mapper.Map<Category>(categoryDto);
        category.UserId = userId;
        
        await EnsureUniqueAsync(category.UserId, category.Name, category.OperationType, category.Id);
        validator.EnsureValid(category);

        await unitOfWork.Categories.AddAsync(category);
        await unitOfWork.CompleteAsync();
        
        return category.Id;
    }

    public async Task UpdateAsync(CategoryDto categoryDto, Guid userId)
    {
        var existingCategory = await GetEntityByIdAsync(categoryDto.Id, userId);
        
        mapper.Map(categoryDto, existingCategory);
        existingCategory.UserId = userId;
        
        await EnsureUniqueAsync(
            existingCategory.UserId,
            existingCategory.Name,
            existingCategory.OperationType,
            existingCategory.Id);
        
        validator.EnsureValid(existingCategory);
        await unitOfWork.CompleteAsync();
    }

    public async Task RemoveAsync(Guid id, Guid userId)
    {
        var category = await GetEntityByIdAsync(id, userId);

        unitOfWork.Categories.Remove(category);
        await unitOfWork.CompleteAsync();
    }    
    
    private async Task<Category> GetEntityByIdAsync(Guid id, Guid userId)
    {
        return await unitOfWork.Categories.GetByIdAsync(id, userId)
               ?? throw new KeyNotFoundException($"A {nameof(Category)} with ID {id} not found.");
    }
    
    private async Task EnsureUniqueAsync(
        Guid userId,
        string name,
        OperationType operationType,
        Guid? excludedId = null)
    {
        var categories = await unitOfWork.Categories.FindAsync(category =>
            category.UserId == userId &&
            category.Name == name &&
            category.OperationType == operationType &&
            (excludedId == null || category.Id != excludedId.Value));

        if (categories.Any())
        {
            throw new InvalidOperationException($"{nameof(Category)} with the same name and type already exists.");
        }
    }
}
