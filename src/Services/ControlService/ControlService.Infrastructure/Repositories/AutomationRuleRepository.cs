using Control.Domain.Entities;
using Control.Domain.Interfaces;

namespace Control.Infrastructure.Repositories;

public class AutomationRuleRepository(SystemDbContext dbContext)
        : BaseRepository<AutomationRuleEntity>(dbContext), IAutomationRuleRepository
{
}

