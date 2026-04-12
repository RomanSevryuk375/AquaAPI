using Control.Application.DTOs.AutomationRule;
using Control.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Control.API.Controllers;

[ApiController]
[Route("api/control/v1/automation-rules")]
public class AutomationRulesController(IAutomationRuleService ruleService) : ControllerBase
{
    private const string NameGetById = "GetRuleById"; [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AutomationRuleResponseDto>>> GetAllRulesAsync(
        [FromQuery] AutomationRuleFilterDto filter,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await ruleService.GetAllRulesAsync(filter, skip, take, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = NameGetById)]
    public async Task<ActionResult<AutomationRuleResponseDto>> GetRuleByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await ruleService.GetRuleByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateRuleAsync(
        [FromBody] AutomationRuleRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var id = await ruleService.CreateRuleAsync(request, cancellationToken);
        var createdData = await ruleService.GetRuleByIdAsync(id, cancellationToken);

        return CreatedAtRoute(
            NameGetById,
            new { id },
            createdData);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateRuleAsync(
        [FromRoute] Guid id,
        [FromBody] AutomationRuleUpdateRequestDto request,
        CancellationToken cancellationToken = default)
    {
        await ruleService.UpdateRuleAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteRuleAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await ruleService.DeleteRuleAsync(id, cancellationToken);
        return NoContent();
    }
}
