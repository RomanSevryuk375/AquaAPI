using Microsoft.AspNetCore.Mvc;
using Telemetry.Application.DTOs;
using Telemetry.Application.Interfaces;

namespace Telemetry.API.Controllers;

[ApiController]
[Route("api/telemetry/v1/data")]
public class TelemetryDataController(ITelemetryDataService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TelemetryDataResponse>>> GetAllDataAsync(
        [FromQuery] TelemetryDataFilterDto filter,
        int skip = 0, 
        int take = 10, 
        CancellationToken cancellationToken = default)
    {
        return Ok(await service.GetAllDataAsync(filter, skip, take, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TelemetryDataResponse>> GetDataByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return Ok(await service.GetDataByIdAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult> AddTelemetryDataAsync(
        TelemetryDataRequest request, 
        CancellationToken cancellationToken)
    {
        var id = await service.AddDataAsync(request, cancellationToken);

        var createdData = await service.GetDataByIdAsync(id, cancellationToken);

        return CreatedAtAction(
            nameof(GetDataByIdAsync),
            new { Id = id },
            createdData);
    }
}
