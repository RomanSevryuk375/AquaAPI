using Microsoft.AspNetCore.Mvc;
using Telemetry.Application.DTOs;
using Telemetry.Application.Interfaces;

namespace Telemetry.API.Controllers;

[ApiController]
[Route("api/telemetry/v1/data")]
public class TelemetryDataController(ITelemetryDataService service) : ControllerBase
{
    private const string NameGetById = "GetTelemetryDataById";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TelemetryDataResponse>>> GetAllDataAsync(
        [FromQuery] TelemetryDataFilterDto filter,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetAllDataAsync(filter, skip, take, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = NameGetById)]
    public async Task<ActionResult<TelemetryDataResponse>> GetDataById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await service.GetDataByIdAsync(id, cancellationToken);
        return Ok(result);
    }
}
