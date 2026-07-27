using BuildingBlocks.Infrastructure.Data;
using Control.Domain.Entities;
using Control.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Control.Infrastructure.Persistence.Repositories;

public sealed class AutomationRuleRepository(ControlDbContext dbContext)
        : BaseRepository<ControlDbContext, AutomationRule>(dbContext), IAutomationRuleRepository
{
    public async Task<AutomationRule?> GetByIdWithConditionsAsync(
        Guid automationRuleId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Rules
            .Include(x => x.Conditions)
            .FirstOrDefaultAsync(x => x.Id == automationRuleId, cancellationToken);
    }

    public async Task<IReadOnlyList<AutomationRule>> GetBySensorIdWithConditionsAsync(
        Guid sensorId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Rules
            .Include(x => x.Conditions)
            .Where(rule => rule.Conditions.Any(c => c.SensorId == sensorId))
            .Where(rule => rule.IsActive)
            .ToListAsync(cancellationToken);
    }
}

