using Microsoft.AspNetCore.Mvc;
using Notification.Application.DTOs.Notification;
using Notification.Application.Interfaces;

namespace Notification.API.Controllers;

[ApiController]
[Route("api/notification/v1/notifications")]
public class NotificationController(
    INotificationService notificationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NotificationResponseDto>>> GetAllNotificationsAsync(
        [FromQuery] NotificationFilterDto filter,
        [FromQuery] int? skip = 0,
        [FromQuery] int? take = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await notificationService
            .GetAllNotificationsAsync(filter, skip, take, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<NotificationResponseDto>> GetNotificationByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await notificationService.GetNotificationByIdAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> MarkNotificationAsReadAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await notificationService.MarkNotificationAsReadAsync(id, cancellationToken);

        return NoContent();
    }
}
