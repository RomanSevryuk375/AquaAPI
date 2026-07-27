using BuildingBlocks.Infrastructure.Data;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;

namespace Notification.Infrastructure.Persistence.Repositories;

public sealed class MaintenanceLogRepository(NotificationDbContext dbContext)
    : BaseRepository<NotificationDbContext, MaintenanceLog>(dbContext), IMaintenanceLogRepository;
