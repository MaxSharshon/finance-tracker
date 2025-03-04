using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BalanceChangeController(IBalanceChangeService balanceChangeService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(balanceChangeService.GetAll());
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
            return Ok(balanceChangeService.GetById(id));
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
    public IActionResult Create([FromBody] BalanceChange balanceChange)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        try
        {
            balanceChangeService.Add(balanceChange);
            return CreatedAtAction(nameof(GetById), new { id = balanceChange.Id }, balanceChange);
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
    public IActionResult Update(Guid id, [FromBody] BalanceChange balanceChange)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (balanceChange.Id != id)
        {
            return BadRequest("ID mismatch");
        }

        try
        {
            balanceChangeService.Update(balanceChange);
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
            balanceChangeService.Remove(id);
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