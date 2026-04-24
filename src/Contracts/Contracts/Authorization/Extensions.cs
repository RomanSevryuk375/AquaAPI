using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Authorization;

public static class Extensions
{
    public static IServiceCollection AddAquaAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(SubPermissions.TankRead, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.TankRead));
            })
            .AddPolicy(SubPermissions.TankCreate, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.TankCreate));
            })
            .AddPolicy(SubPermissions.TankUpdate, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.TankUpdate));
            })
            .AddPolicy(SubPermissions.TankDelete, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.TankDelete));
            })
            .AddPolicy(SubPermissions.DeviceControl, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.DeviceControl));
            })
            .AddPolicy(SubPermissions.DeviceEditManual, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.DeviceEditManual));
            })
            .AddPolicy(SubPermissions.AutoRuleCreate, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.AutoRuleCreate));
            })
            .AddPolicy(SubPermissions.AutoScheduleCreate, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.AutoScheduleCreate));
            })
            .AddPolicy(SubPermissions.VacationMode, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.VacationMode));
            })
            .AddPolicy(SubPermissions.TelemetryView, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.TelemetryView));
            })
            .AddPolicy(SubPermissions.AnalyticsHistory, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.AnalyticsHistory));
            })
            .AddPolicy(SubPermissions.DiagnosticsFull, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.DiagnosticsFull));
            })
            .AddPolicy(SubPermissions.DataRealtime, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.DataRealtime));
            })
            .AddPolicy(SubPermissions.MaintenanceLogRead, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.MaintenanceLogRead));
            })
            .AddPolicy(SubPermissions.MaintenanceLogWrite, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.MaintenanceLogWrite));
            })
            .AddPolicy(SubPermissions.ReminderManage, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.ReminderManage));
            })
            .AddPolicy(SubPermissions.EmailAlerts, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.EmailAlerts));
            })
            .AddPolicy(SubPermissions.TelegramAlerts, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.TelegramAlerts));
            })
            .AddPolicy(SubPermissions.AccountView, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.AccountView));
            })
            .AddPolicy(SubPermissions.AccountUpdate, policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(CustomClaims.Permissions, SubPermissions.AccountUpdate));
            });

        return services;
    }
}
