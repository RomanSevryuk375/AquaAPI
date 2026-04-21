using Microsoft.AspNetCore.Mvc;
using Notification.Application.DTOs.MaintenanceLog;
using Notification.Application.Interfaces;
using System.Security.Claims;

namespace Notification.API.Controllers;

[ApiController]
[Route("api/notification/v1/maintenance-logs")]
public class MaintenanceLogsController(IMaintenanceLogService logService) : ControllerBase
{
    private const string NameGetById = "GetLogById";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaintenanceLogResponseDto>>> GetAllLogsAsync(
        [FromQuery] MaintenanceLogFilterDto filter,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await logService.GetAllLogs(filter, skip, take, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = NameGetById)]
    public async Task<ActionResult<MaintenanceLogResponseDto>> GetLogByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await logService.GetLogById(id, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> AddLogAsync(
        [FromBody] MaintenanceLogRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var id = await logService.AddLogAsync(request, cancellationToken);
        var createdData = await logService.GetLogById(id, cancellationToken);

        return CreatedAtRoute(
            NameGetById,
            new { id  = id },
            createdData);
    }
}