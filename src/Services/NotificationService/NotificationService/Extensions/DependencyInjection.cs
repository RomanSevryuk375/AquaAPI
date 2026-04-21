using Microsoft.EntityFrameworkCore;
using Notification.Application.Extensions;
using Notification.Infrastructure;
using Notification.Infrastructure.Extensions;

namespace Notification.API.Extensions;

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

        services.AddRepositories(configuration);
        services.AddServices();
        services.AddQuartzJobs();
        services.AddRabbitMq(configuration);

        return services;
    }
}
