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

    public async Task<CategorySuggestionDto?> SuggestAsync(
        Guid userId,
        string description,
        decimal amount,
        string operationType)
    {
        if (!TryParseOperationType(operationType, out var parsedType))
        {
            return null;
        }

        var categories = await GetSuggestionCategoriesAsync(userId, parsedType);

        if (categories.Count == 0)
        {
            return null;
        }

        return FindRuleBasedSuggestion(description, categories, parsedType)
            ?? CreateFallbackSuggestion(categories, amount);
    }

    private static IReadOnlyList<(string[] CategoryNames, string[] Keywords)> GetSuggestionRules(OperationType operationType)
    {
        return operationType == OperationType.Income
            ? [
                (["salary", "зарплата", "income", "дохід"], ["salary", "payroll", "bonus", "зарплата", "аванс"])
            ]
            : [
                (["food", "groceries", "їжа", "продукти"], ["market", "grocery", "silpo", "atb", "food", "cafe", "coffee", "продукти", "кава"]),
                (["transport", "транспорт"], ["uber", "bolt", "taxi", "bus", "metro", "fuel", "gas", "таксі", "метро"]),
                (["home", "household", "дім"], ["rent", "utility", "utilities", "electricity", "internet", "комунальні", "оренда"]),
                (["health", "здоров"], ["pharmacy", "doctor", "clinic", "аптека", "лікар"]),
                (["entertainment", "розва"], ["netflix", "spotify", "cinema", "game", "кіно"])
            ];
    }
    
    private static bool TryParseOperationType(string operationType, out OperationType parsedType)
    {
        return Enum.TryParse(operationType, true, out parsedType);
    }

    private async Task<List<Category>> GetSuggestionCategoriesAsync(Guid userId, OperationType operationType)
    {
        return (await unitOfWork.Categories.GetByUserAsync(userId))
            .Where(category => category.OperationType == operationType)
            .ToList();
    }
    
    private static CategorySuggestionDto? FindRuleBasedSuggestion(
        string description,
        IReadOnlyCollection<Category> categories,
        OperationType operationType)
    {
        var normalizedDescription = description.ToLowerInvariant();
        var rules = GetSuggestionRules(operationType);

        foreach (var (categoryNames, keywords) in rules)
        {
            var matchedKeyword = FindMatchedKeyword(normalizedDescription, keywords);

            if (matchedKeyword is null)
            {
                continue;
            }

            var category = FindMatchingCategory(categories, categoryNames);

            if (category is null)
            {
                continue;
            }

            return CreateSuggestion(category, 0.9m, matchedKeyword);
        }

        return null;
    }
    
    private static string? FindMatchedKeyword(string normalizedDescription, IEnumerable<string> keywords)
    {
        return keywords.FirstOrDefault(keyword =>
            normalizedDescription.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    private static Category? FindMatchingCategory(IEnumerable<Category> categories, IEnumerable<string> categoryNames)
    {
        return categories.FirstOrDefault(category =>
            categoryNames.Any(name =>
                category.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));
    }
    
    private static CategorySuggestionDto CreateFallbackSuggestion(IReadOnlyList<Category> categories, decimal amount)
    {
        var fallback = categories[0];

        return CreateSuggestion(
            fallback,
            amount > 0 ? 0.45m : 0.35m,
            null);
    }

    private static CategorySuggestionDto CreateSuggestion(
        Category category,
        decimal confidence,
        string? matchedKeyword)
    {
        return new CategorySuggestionDto
        {
            CategoryId = category.Id,
            CategoryName = category.Name,
            OperationType = category.OperationType,
            Confidence = confidence,
            MatchedKeyword = matchedKeyword
        };
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
