using AutoMapper;
using FinanceTracker.API.Contracts;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BalanceChangeController(IBalanceChangeService balanceChangeService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(mapper.Map<IEnumerable<BalanceChangeResponse>>(balanceChangeService.GetAllAsync()));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            return Ok(mapper.Map<BalanceChangeResponse>(balanceChangeService.GetById(id)));
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
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] BalanceChangeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var balanceChangeDto = mapper.Map<BalanceChangeDto>(request);
            balanceChangeDto.Id = id;
            balanceChangeService.UpdateAsync(balanceChangeDto);
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
    public IActionResult Delete(Guid id)
    {
        try
        {
            balanceChangeService.RemoveAsync(id);
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