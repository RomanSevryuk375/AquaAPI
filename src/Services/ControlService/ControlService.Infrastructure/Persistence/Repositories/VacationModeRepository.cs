using BuildingBlocks.Infrastructure.Data;
using Control.Domain.Entities;
using Control.Domain.Interfaces;

namespace Control.Infrastructure.Persistence.Repositories;

public sealed class VacationModeRepository(ControlDbContext dbContext)
    : BaseRepository<ControlDbContext, VacationMode>(dbContext), IVacationModeRepository;
