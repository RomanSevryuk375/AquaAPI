using Device.Application.DTOs.Relay;
using Device.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Device.API.Controllers;

[ApiController]
[Route("api/device/v1/relays")]
public class RelaysController(
    IRelayService relayService) : ControllerBase
{
    private const string NameGetById = "GetRelayByIdAsync";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RelayResponseDto>>> GetAllRelaysAsync(
        [FromQuery] RelayFilterDto filter,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await relayService.GetAllRelaysAsync(
            filter, 
            skip, 
            take, 
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = NameGetById)]
    public async Task<ActionResult<RelayResponseDto>> GetRelayByIdAsync(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        var result = await relayService.GetRelayByIdAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> AddRelayAsync(
        [FromBody] RelayRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var id = await relayService.AddRelayAsync(request, cancellationToken);

        var createdData = await relayService.GetRelayByIdAsync(id, cancellationToken);

        return CreatedAtRoute(
            NameGetById, 
            new { id = id },
            createdData);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateRelayAsync(
        [FromRoute] Guid id,
        [FromBody] RelayUpdateRequestDto request,
        CancellationToken cancellationToken = default)
    {
        await relayService.UpdateRelayAsync(id, request, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/mode")]
    public async Task<ActionResult<bool>> ToggleRelayModeAsync(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        var result = await relayService.ToggleRelayModeAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/set-state")]
    public async Task<ActionResult<bool>> SetRelayStateAsync(
        [FromRoute] Guid id,
        [FromQuery] bool state,
        CancellationToken cancellationToken = default)
    {
        var result = await relayService.SetRelayStateAsync(id, state, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/state")]
    public async Task<ActionResult<bool>> ToggleRelayStateAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await relayService.ToggleRelayStateAsync(id, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteRelayAsync(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken = default)
    {
        await relayService.DeleteRelayAsync(id, cancellationToken);

        return NoContent();
    }
}
