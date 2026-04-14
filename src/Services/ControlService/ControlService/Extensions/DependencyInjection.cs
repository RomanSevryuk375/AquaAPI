using Control.Application.Extensions;
using Control.Infrastructure;
using Control.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Control.API.Extensions;

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
        services.AddQuartzJobs();
        services.AddRabbitMq(configuration);

        return services;
    }
}
