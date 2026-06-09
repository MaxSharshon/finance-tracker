using AutoMapper;
using FinanceTracker.API.Extensions;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Contracts.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController(INotificationService notificationService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId();
        var notifications = await notificationService.GetAllAsync(userId);
        return Ok(mapper.Map<IEnumerable<NotificationResponse>>(notifications));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.GetUserId();
        var notification = await notificationService.GetByIdAsync(id, userId);
        return Ok(mapper.Map<NotificationResponse>(notification));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NotificationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        var notificationDto = mapper.Map<NotificationDto>(request);
        notificationDto.Id = await notificationService.AddAsync(notificationDto, userId);

        return CreatedAtAction(nameof(GetById),
            new { id = notificationDto.Id },
            mapper.Map<NotificationResponse>(notificationDto));
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] NotificationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        var notificationDto = mapper.Map<NotificationDto>(request);
        notificationDto.Id = id;

        await notificationService.UpdateAsync(notificationDto, userId);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.GetUserId();
        await notificationService.RemoveAsync(id, userId);
        return NoContent();
    }
}
