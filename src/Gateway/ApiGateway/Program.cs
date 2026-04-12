var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger-docs/telemetry/swagger/v1/swagger.json", "Telemetry API");
    options.SwaggerEndpoint("/swagger-docs/device/swagger/v1/swagger.json", "Device API");
    options.SwaggerEndpoint("/swagger-docs/control/swagger/v1/swagger.json", "Control API");
});

app.MapReverseProxy();

app.Run();
