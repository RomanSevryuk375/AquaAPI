using Contracts.Enums;
using Device.Application.DTOs.Sensor;
using Device.Application.DTOs.Telemetry;
using Device.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Device.API.Controllers;

[ApiController]
[Route("api/device/v1/sensors")]
public class SensorsController(
    ISensorService sensorService) : ControllerBase
{
    private const string NameGetById = "GetSensorByIdAsync";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SensorResponseDto>>> GetAllSensorsAsync(
        [FromQuery] SensorFilterDto filter,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await sensorService.GetAllSensorsAsync(
            filter,
            skip, 
            take, 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = NameGetById)]
    public async Task<ActionResult<SensorResponseDto>> GetSensorByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await sensorService.GetSensorByIdAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> AddSensorAsync(
        [FromBody] SensorRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var id = await sensorService.AddSensorAsync(request, cancellationToken);

        var createdData = await sensorService.GetSensorByIdAsync(id, cancellationToken);

        return CreatedAtRoute(
            NameGetById,
            new { id = id },
            createdData);
    }

    [HttpPost("telemetry")] 
    public async Task<ActionResult> ReceiveBatchTelemetryAsync(
    [FromBody] TelemetryBatchRequest request,
    CancellationToken cancellationToken)
    {
        var result = await sensorService.ProcessTelemetryBatchAsync(request, cancellationToken);

        return Accepted(new
        {
            result.AcceptedCount,
            result.ValidationErrors,
            result.SkippedCount
        });
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateSensorAsync(
        [FromRoute] Guid id,
        [FromBody] SensorUpdateRequestDto reuqest,
        CancellationToken cancellationToken = default)
    {
        await sensorService.UpdateSensorAsync(id, reuqest, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/state")]
    public async Task<ActionResult> SetSensorStateAsync(
        [FromRoute] Guid id,
        [FromQuery] SensorStateEnum state,
        CancellationToken cancellationToken = default)
    {
        await sensorService.SetSensorStateAsync(id, state, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteSensorAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await sensorService.DeleteSensorAsync(id, cancellationToken);

        return NoContent();
    }
}
