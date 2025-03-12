using AutoMapper;
using FinanceTracker.API.Contracts;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api")]
[ApiController]
public class ReportsController(IReportsService reportsService, IMapper mapper) : ControllerBase
{
    [HttpGet("daily-report")]
    public async Task<IActionResult> GetDailyReportAsync([FromQuery] string date)
    {
        if (!DateTime.TryParse(date, out var reportDate))
        {
            return BadRequest("Invalid date format.");
        }

        try
        {
            var reportDto = await reportsService.GetDailyReportAsync(reportDate);
            var response = mapper.Map<DailyReportResponse>(reportDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpGet("date-period-report")]
    public async Task<IActionResult> GetDatePeriodReportAsync([FromQuery] string startDate, [FromQuery] string endDate)
    {
        if (!DateTime.TryParse(startDate, out var reportStartDate) &
            !DateTime.TryParse(endDate, out var reportEndDate))
        {
            return BadRequest("Invalid date format.");
        }

        try
        {
            var reportDto = await reportsService.GetDatePeriodReportAsync(reportStartDate, reportEndDate);
            var response = mapper.Map<DatePeriodReportResponse>(reportDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}