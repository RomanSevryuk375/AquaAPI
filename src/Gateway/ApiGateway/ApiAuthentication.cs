using Contracts.JwtToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiGateway;

public static class ApiAuthentication
{
    public static IServiceCollection AddApiAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection(nameof(JwtOptions));
        var jwtOptions = jwtSection.Get<JwtOptions>();

        if (jwtOptions == null || string.IsNullOrEmpty(jwtOptions.SecretKey))
        {
            throw new Exception("JWT configuration missing or invalid");
        }

        services.Configure<JwtOptions>(jwtSection);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });

        return services;
    }
}
