using Microsoft.EntityFrameworkCore;
using Telemetry.Application.Extensions;
using Telemetry.Infrastructure;
using Telemetry.Infrastructure.Extensions;

namespace Telemetry.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddControllers();

        services.AddDbContext<SystemDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(SystemDbContext)))
                .UseSnakeCaseNamingConvention();
        });

        services.AddRepositories();
        services.AddServices();

        return services;
    }
}
