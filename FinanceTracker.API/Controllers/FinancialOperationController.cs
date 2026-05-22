using AutoMapper;
using FinanceTracker.API.Contracts.FinancialOperations;
using FinanceTracker.API.Extensions;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
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
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var userId = User.GetUserId();
            var operationDtos = await financialOperationService.GetAllAsync(userId);
            
            return Ok(mapper.Map<IEnumerable<FinancialOperationResponse>>(operationDtos));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var userId = User.GetUserId();
            var operationDto = await financialOperationService.GetByIdAsync(id, userId);
            
            return Ok(mapper.Map<FinancialOperationResponse>(operationDto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] FinancialOperationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.GetUserId();
            var operationDto = mapper.Map<FinancialOperationDto>(request);
            operationDto.Id = await financialOperationService.AddAsync(operationDto, userId);
            
            return CreatedAtAction(nameof(GetById), new { id = operationDto.Id },
                mapper.Map<FinancialOperationResponse>(operationDto));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] FinancialOperationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var userId = User.GetUserId();
            var operationDto = mapper.Map<FinancialOperationDto>(request);
            operationDto.Id = id;
            
            await financialOperationService.UpdateAsync(operationDto, userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var userId = User.GetUserId();
            await financialOperationService.RemoveAsync(id, userId);
            
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}