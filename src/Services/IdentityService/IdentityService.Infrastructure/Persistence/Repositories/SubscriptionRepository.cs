using BuildingBlocks.Infrastructure.Data;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Interfaces;

namespace IdentityService.Infrastructure.Persistence.Repositories;

public sealed class SubscriptionRepository(IdentityDbContext dbContext)
    : BaseRepository<IdentityDbContext, Subscription>(dbContext), ISubscriptionRepository;
