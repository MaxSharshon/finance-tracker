using AutoMapper;
using FinanceTracker.API.Extensions;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Contracts.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController(ICategoryService categoryService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var userId = User.GetUserId();
        var categories = await categoryService.GetAllAsync(userId);
        return Ok(mapper.Map<IEnumerable<CategoryResponse>>(categories));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var category = await categoryService.GetByIdAsync(id, userId);
        return Ok(mapper.Map<CategoryResponse>(category));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        var categoryDto = mapper.Map<CategoryDto>(request);
        categoryDto.Id = await categoryService.AddAsync(categoryDto, userId);

        return CreatedAtAction(nameof(GetById),
            new { id = categoryDto.Id },
            mapper.Map<CategoryResponse>(categoryDto));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CategoryRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        var categoryDto = mapper.Map<CategoryDto>(request);
        categoryDto.Id = id;
        
        await categoryService.UpdateAsync(categoryDto, userId);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        await categoryService.RemoveAsync(id, userId);
        return NoContent();
    }
}
