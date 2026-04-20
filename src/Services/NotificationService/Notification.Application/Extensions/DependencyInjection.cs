using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Interfaces;
using Notification.Application.Services;

namespace Notification.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAquariumServiceFromEvent, AquariumServiceFromEvent>();
        services.AddScoped<IMaintenanceLogService, MaintenanceLogService>();
        services.AddScoped<INotificationSender, NotificationSender>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IReminderProcessor, ReminderProcessor>();
        services.AddScoped<IReminderService, ReminderService>();
        services.AddScoped<IUnpublishedNoticeProcessor, UnpublishedNoticeProcessor>();
        services.AddScoped<IUserServiceFromEvent, UserServiceFromEvent>();

        return services;
    }
}
