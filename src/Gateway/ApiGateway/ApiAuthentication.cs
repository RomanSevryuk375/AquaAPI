using BuildingBlocks.Presentation.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiGateway;

public static class ApiAuthentication
{
    public static IServiceCollection AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        IConfigurationSection jwtSection = configuration.GetSection(JwtOptions.SectionName);
        JwtOptions? jwtOptions = jwtSection.Get<JwtOptions>();

        if (jwtOptions is null || string.IsNullOrWhiteSpace(jwtOptions.SecretKey))
        {
            throw new InvalidOperationException("JWT configuration missing or invalid.");
        }

        services.Configure<JwtOptions>(jwtSection);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        BuildingBlocks.Presentation.Authorization.Extensions.ConfigureJwtBearer(options, jwtOptions);
                    });

        return services;
    }
}
