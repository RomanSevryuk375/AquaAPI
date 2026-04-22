using Microsoft.AspNetCore.Mvc;
using Notification.Application.DTOs.Reminder;
using Notification.Application.Interfaces;

namespace Notification.API.Controllers;

[ApiController]
[Route("api/notification/v1/reminders")]
public class RemindersController(IReminderService reminderService) : ControllerBase
{
    private const string NameGetById = "GetReminderById";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReminderResponseDto>>> GetAllRemindersAsync(
        [FromQuery] ReminderFilterDto filter,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await reminderService.GetAllRemindersAsync(filter, skip, take, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = NameGetById)]
    public async Task<ActionResult<ReminderResponseDto>> GetReminderByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await reminderService.GetReminderByIdAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> AddReminderAsync(
        [FromBody] ReminderRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var id = await reminderService.AddReminderAsync(request, cancellationToken);
        var createdData = await reminderService.GetReminderByIdAsync(id, cancellationToken);

        return CreatedAtRoute(
            NameGetById,
            new { id = id },
            createdData);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateReminderAsync(
        [FromRoute] Guid id,
        [FromBody] ReminderUpdateRequestDto request,
        CancellationToken cancellationToken = default)
    {
        await reminderService.UpdateReminderAsync(id, request, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<ActionResult> CompleteTaskAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await reminderService.ReminderCompleteTaskAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteReminderAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await reminderService.DeleteReminderAsync(id, cancellationToken);

        return NoContent();
    }
}