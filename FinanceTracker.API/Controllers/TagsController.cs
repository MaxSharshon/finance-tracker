using AutoMapper;
using FinanceTracker.API.Contracts.Tags;
using FinanceTracker.API.Extensions;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TagsController(ITagService tagService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var usedId = User.GetUserId();
        var tags = await tagService.GetAllAsync(usedId);
        return Ok(mapper.Map<IEnumerable<TagResponse>>(tags));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var usedId = User.GetUserId();
        var tag = await tagService.GetByIdAsync(id, usedId);
        return Ok(mapper.Map<TagResponse>(tag));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TagRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        var tagDto = mapper.Map<TagDto>(request);
        tagDto.Id = await tagService.AddAsync(tagDto, userId);
        
        return CreatedAtAction(nameof(GetById),
            new { id = tagDto.Id },
            mapper.Map<TagResponse>(tagDto));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TagRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = User.GetUserId();
        var tagDto = mapper.Map<TagDto>(request);
        tagDto.Id = id;
        
        await tagService.UpdateAsync(tagDto, userId);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var usedId = User.GetUserId();
        await tagService.RemoveAsync(id, usedId);
        return NoContent();
    }
}
