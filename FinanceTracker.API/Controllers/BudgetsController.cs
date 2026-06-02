using AutoMapper;
using FinanceTracker.API.Contracts.Budgets;
using FinanceTracker.API.Extensions;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BudgetsController(IBudgetService budgetService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId();
        var budgets = await budgetService.GetAllAsync(userId);
        return Ok(mapper.Map<IEnumerable<BudgetResponse>>(budgets));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var budget = await budgetService.GetByIdAsync(id, userId);
        return Ok(mapper.Map<BudgetResponse>(budget));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BudgetRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        var budgetDto = mapper.Map<BudgetDto>(request);
        budgetDto.Id = await budgetService.AddAsync(budgetDto, userId);

        return CreatedAtAction(nameof(GetById),
            new { id = budgetDto.Id },
            mapper.Map<BudgetResponse>(budgetDto));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BudgetRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        var budgetDto = mapper.Map<BudgetDto>(request);
        budgetDto.Id = id;

        await budgetService.UpdateAsync(budgetDto, userId);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        await budgetService.RemoveAsync(id, userId);
        return NoContent();
    }

    [HttpGet("{id:guid}/members")]
    public async Task<IActionResult> GetMembers(Guid id)
    {
        var userId = User.GetUserId();
        var members = await budgetService.GetMembersAsync(id, userId);
        return Ok(mapper.Map<IEnumerable<BudgetMemberResponse>>(members));
    }

    [HttpPost("{id:guid}/members")]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] BudgetMemberRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        await budgetService.AddMemberAsync(id, request.UserId, userId);
        return NoContent();
    }

    [HttpDelete("{id:guid}/members/{memberUserId:guid}")]
    public async Task<IActionResult> RemoveMember(Guid id, Guid memberUserId)
    {
        var userId = User.GetUserId();
        await budgetService.RemoveMemberAsync(id, memberUserId, userId);
        return NoContent();
    }
}
