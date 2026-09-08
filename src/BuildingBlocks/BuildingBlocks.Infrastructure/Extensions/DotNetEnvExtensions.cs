namespace BuildingBlocks.Infrastructure.Extensions;

public static class DotNetEnvExtensions
{
    public static void LoadDotNetEnv()
    {
        DotNetEnv.Env.TraversePath().Load();

        string? secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
        if (!string.IsNullOrEmpty(secretKey))
        {
            Environment.SetEnvironmentVariable("JwtOptions__SecretKey", secretKey);
        }

        string? issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        if (!string.IsNullOrEmpty(issuer))
        {
            Environment.SetEnvironmentVariable("JwtOptions__Issuer", issuer);
        }

        string? audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        if (!string.IsNullOrEmpty(audience))
        {
            Environment.SetEnvironmentVariable("JwtOptions__Audience", audience);
        }

        string? expiresHours = Environment.GetEnvironmentVariable("JWT_EXPIRES_HOURS");
        if (!string.IsNullOrEmpty(expiresHours))
        {
            Environment.SetEnvironmentVariable("JwtOptions__ExpiresHours", expiresHours);
        }
    }
}
