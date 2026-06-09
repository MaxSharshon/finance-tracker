using AutoMapper;
using FinanceTracker.API.Extensions;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Contracts.FinancialOperations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FinancialOperationController(IFinancialOperationService financialOperationService, IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FinancialOperationFilterRequest filter)
    {
        var userId = User.GetUserId();
        var filterDto = mapper.Map<FinancialOperationFilterDto>(filter);
        var operationDtos = await financialOperationService.GetAllAsync(userId, filterDto);

        return Ok(mapper.Map<IEnumerable<FinancialOperationResponse>>(operationDtos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var operationDto = await financialOperationService.GetByIdAsync(id, userId);
        
        return Ok(mapper.Map<FinancialOperationResponse>(operationDto));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FinancialOperationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        var operationDto = mapper.Map<FinancialOperationDto>(request);
        operationDto.Id = await financialOperationService.AddAsync(operationDto, userId);
        
        return CreatedAtAction(nameof(GetById), new { id = operationDto.Id },
            mapper.Map<FinancialOperationResponse>(operationDto));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] FinancialOperationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = User.GetUserId();
        var operationDto = mapper.Map<FinancialOperationDto>(request);
        operationDto.Id = id;
        
        await financialOperationService.UpdateAsync(operationDto, userId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        await financialOperationService.RemoveAsync(id, userId);
        
        return NoContent();
    }
}
