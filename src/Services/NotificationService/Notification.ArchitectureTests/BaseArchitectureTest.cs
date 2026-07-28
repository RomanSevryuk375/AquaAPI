using System.Reflection;
using Notification.API.Endpoints.Reminders;
using Notification.Application.Features.BackgroundJobs.Commands.ProcessUnpublishedNotices;
using Notification.Domain.Entities;
using Notification.Infrastructure.Persistence;

namespace Notification.ArchitectureTests;

public abstract class BaseArchitectureTest
{
    protected static readonly Assembly DomainAssembly = typeof(User).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(ProcessUnpublishedNoticesCommand).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(NotificationDbContext).Assembly;
    protected static readonly Assembly ApiAssembly = typeof(AddReminderEndpoint).Assembly;
}
