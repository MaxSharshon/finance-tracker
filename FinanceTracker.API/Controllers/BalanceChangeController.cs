using AutoMapper;
using FinanceTracker.API.Contracts.BalanceChanges;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BalanceChangeController(IBalanceChangeService balanceChangeService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(mapper.Map<IEnumerable<BalanceChangeResponse>>(await balanceChangeService.GetAllAsync()));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            return Ok(mapper.Map<BalanceChangeResponse>(await balanceChangeService.GetById(id)));
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

    [HttpGet("unused")]
    public async Task<IActionResult> GetUnusedAsync()
    {
        try
        {
            return Ok(mapper.Map<IEnumerable<BalanceChangeResponse>>(await balanceChangeService.GetUnusedAsync()));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] BalanceChangeRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        try
        {
            var balanceChangeDto = mapper.Map<BalanceChangeDto>(request);
            balanceChangeDto.Id = await balanceChangeService.AddAsync(balanceChangeDto);
            return CreatedAtAction(nameof(GetById), new { id = balanceChangeDto.Id },
                mapper.Map<BalanceChangeResponse>(balanceChangeDto));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BalanceChangeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var balanceChangeDto = mapper.Map<BalanceChangeDto>(request);
            balanceChangeDto.Id = id;
            await balanceChangeService.UpdateAsync(balanceChangeDto);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await balanceChangeService.RemoveAsync(id);
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