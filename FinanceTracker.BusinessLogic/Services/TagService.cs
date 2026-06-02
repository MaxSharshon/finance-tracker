using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Extensions;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using FluentValidation;

namespace FinanceTracker.BusinessLogic.Services;

public class TagService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<Tag> validator) 
    : ITagService
{ 
    public async Task<IEnumerable<TagDto>> GetAllAsync(Guid userId)
    {
        var tags = await unitOfWork.Tags.GetByUserAsync(userId);
        return mapper.Map<IEnumerable<TagDto>>(tags);
    }
    
    public async Task<TagDto> GetByIdAsync(Guid id, Guid userId)
    {
        return mapper.Map<TagDto>(await GetEntityByIdAsync(id, userId));
    }

    public async Task<Guid> AddAsync(TagDto tagDto, Guid userId)
    {
        var tag = mapper.Map<Tag>(tagDto);
        tag.UserId = userId;
        
        await EnsureUniqueAsync(tag.UserId, tag.Name, tag.Id);
        validator.EnsureValid(tag);
        
        await unitOfWork.Tags.AddAsync(tag);
        await unitOfWork.CompleteAsync();
        
        return tag.Id;
    }

    public async Task UpdateAsync(TagDto tagDto, Guid userId)
    {
        var existingTag = await GetEntityByIdAsync(tagDto.Id, userId);
        
        mapper.Map(tagDto, existingTag);
        existingTag.UserId = userId;
        
        await EnsureUniqueAsync(existingTag.UserId, existingTag.Name, existingTag.Id);
        validator.EnsureValid(existingTag);

        await unitOfWork.CompleteAsync();
    }

    public async Task RemoveAsync(Guid id, Guid userId)
    {
        var tag = await GetEntityByIdAsync(id, userId);
        unitOfWork.Tags.Remove(tag);
        await unitOfWork.CompleteAsync();
    }
    
    private async Task<Tag> GetEntityByIdAsync(Guid id, Guid userId)
    {
        return await unitOfWork.Tags.GetByIdAsync(id, userId)
               ?? throw new KeyNotFoundException($"A {nameof(Tag)} with ID {id} not found.");
    }
    
    private async Task EnsureUniqueAsync(Guid userId, string name, Guid? excludedId = null)
    {
        var tags = await unitOfWork.Tags.FindAsync(tag =>
            tag.UserId == userId &&
            tag.Name == name &&
            (excludedId == null || tag.Id != excludedId.Value));

        if (tags.Any())
            throw new InvalidOperationException($"{nameof(Tag)} with the same name already exists.");
    }
}
