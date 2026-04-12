using Control.Application.DTOs.AutomationRule;

namespace Control.Application.Interfaces;

public interface IAutomationRuleService
{
    Task<Guid> CreateRuleAsync(
        AutomationRuleRequestDto request, 
        CancellationToken cancellationToken);

    Task DeleteRuleAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AutomationRuleResponseDto>> GetAllRulesAsync(
        AutomationRuleFilterDto filter,
        int? skip, 
        int? take, 
        CancellationToken cancellationToken);

    Task<AutomationRuleResponseDto> GetRuleByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task UpdateRuleAsync(
        Guid id, 
        AutomationRuleUpdateRequestDto request, 
        CancellationToken cancellationToken);
}