using ApiGateway;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMySwaggerGen();

builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddAquaAuthorizationPolicies();
builder.Services.AddAuthorization();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger-docs/telemetry/swagger/v1/swagger.json", "Telemetry API");
    options.SwaggerEndpoint("/swagger-docs/device/swagger/v1/swagger.json", "Device API");
    options.SwaggerEndpoint("/swagger-docs/control/swagger/v1/swagger.json", "Control API");
    options.SwaggerEndpoint("/swagger-docs/identity/swagger/v1/swagger.json", "Identity API");
    options.SwaggerEndpoint("/swagger-docs/notification/swagger/v1/swagger.json", "Notification API");
    options.SwaggerEndpoint("/swagger-docs/firmware/swagger/doc.json", "Firmware API");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

await app.RunAsync();
