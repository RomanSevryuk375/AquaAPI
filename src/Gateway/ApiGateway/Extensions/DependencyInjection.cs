using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Extensions;

namespace ApiGateway.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"));

        services.AddEndpointsApiExplorer();
        services.AddMySwaggerGen();

        services.AddApiAuthentication(configuration);
        services.AddAquaAuthorizationPolicies();
        services.AddAuthorization();
        services.AddProblemDetails();

        return services;
    }

    public static WebApplication AddGatewayConfiguration(this WebApplication application)
    {
        application.UseExceptionHandler();

        application.UseSwagger();
        application.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger-docs/telemetry/swagger/v1/swagger.json", "Telemetry API");
            options.SwaggerEndpoint("/swagger-docs/device/swagger/v1/swagger.json", "Device API");
            options.SwaggerEndpoint("/swagger-docs/control/swagger/v1/swagger.json", "Control API");
            options.SwaggerEndpoint("/swagger-docs/identity/swagger/v1/swagger.json", "Identity API");
            options.SwaggerEndpoint("/swagger-docs/notification/swagger/v1/swagger.json", "Notification API");
            options.SwaggerEndpoint("/swagger-docs/firmware/swagger/doc.json", "Firmware API");
        });

        application.UseAuthentication();
        application.UseAuthorization();

        application.MapReverseProxy();

        return application;
    }
}
