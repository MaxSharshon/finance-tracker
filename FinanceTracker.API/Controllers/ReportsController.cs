using AutoMapper;
using FinanceTracker.API.Contracts.Reports;
using FinanceTracker.API.Extensions;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api")]
[ApiController]
[Authorize]
public class ReportsController(IReportsService reportsService, IMapper mapper) : ControllerBase
{
    [HttpGet("daily-report")]
    public async Task<IActionResult> GetDailyReportAsync([FromQuery] DateTime date)
    {
        var userId = User.GetUserId();
        var reportDto = await reportsService.GetDailyReportAsync(date, userId);
        var response = mapper.Map<DailyReportResponse>(reportDto);
        return Ok(response);
    }
    
    [HttpGet("date-period-report")]
    public async Task<IActionResult> GetDatePeriodReportAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var userId = User.GetUserId();
        var reportDto = await reportsService.GetDatePeriodReportAsync(startDate, endDate, userId);
        var response = mapper.Map<DatePeriodReportResponse>(reportDto);
        return Ok(response);
    }
}
