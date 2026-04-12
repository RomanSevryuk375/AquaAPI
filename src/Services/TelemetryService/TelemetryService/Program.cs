using Microsoft.EntityFrameworkCore;
using Telemetry.API.Extensions;
using Telemetry.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("SystemDbContext")!)
    .AddRabbitMQ(new Uri(builder.Configuration["MessageBroker:Host"]!));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SystemDbContext>();
    context.Database.Migrate();
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
