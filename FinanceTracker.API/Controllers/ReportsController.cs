using AutoMapper;
using FinanceTracker.API.Contracts.Reports;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api")]
[ApiController]
public class ReportsController(IReportsService reportsService, IMapper mapper) : ControllerBase
{
    [HttpGet("daily-report")]
    public async Task<IActionResult> GetDailyReportAsync([FromQuery] DateTime date)
    {
        try
        {
            var reportDto = await reportsService.GetDailyReportAsync(date);
            var response = mapper.Map<DailyReportResponse>(reportDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpGet("date-period-report")]
    public async Task<IActionResult> GetDatePeriodReportAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var reportDto = await reportsService.GetDatePeriodReportAsync(startDate, endDate);
            var response = mapper.Map<DatePeriodReportResponse>(reportDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}