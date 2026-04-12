using Control.Application.DTOs.Aquarium;
using Control.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Control.API.Controllers;

[ApiController]
[Route("api/control/v1/aquariums")]
public class AquariumsController(IAquariumService aquariumService) : ControllerBase
{
    private const string NameGetById = "GetAquariumById";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AquariumResponseDto>>> GetAllAquariumsAsync(
        [FromQuery] AquariumFilterDto filter,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await aquariumService.GetAllAquariumsAsync(filter, skip, take, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = NameGetById)]
    public async Task<ActionResult<AquariumResponseDto>> GetAquariumByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await aquariumService.GetAquariumByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAquariumAsync(
        [FromBody] AquariumRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var id = await aquariumService.CreateAquariumAsync(request, cancellationToken);
        var createdData = await aquariumService.GetAquariumByIdAsync(id, cancellationToken);

        return CreatedAtRoute(
            NameGetById,
            new { id },
            createdData);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAquariumAsync([FromRoute] Guid id,
        [FromBody] AquariumUpdateRequestDto request,
        CancellationToken cancellationToken = default)
    {
        await aquariumService.UpdateAquariumAsync(id, request, cancellationToken);
        return NoContent(); 
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAquariumAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await aquariumService.DeleteAquariumAsync(id, cancellationToken);
        return NoContent(); 
    }
}
