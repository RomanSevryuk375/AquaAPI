using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace BuildingBlocks.Infrastructure.Extensions;

public static class LoggingHostExtension
{
    public static WebApplicationBuilder AddGlobalElkLogging(this WebApplicationBuilder builder, string applicationName)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
           .ReadFrom.Configuration(context.Configuration)
           .Enrich.WithExceptionDetails()
           .Enrich.WithProperty("Application", applicationName)
           .WriteTo.Elasticsearch(
               new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]!))
               {
                   IndexFormat = $"aquasmart-logs-{context.HostingEnvironment.EnvironmentName?
                   .ToLower()
                   .Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                   AutoRegisterTemplate = true,
               }),
           preserveStaticLogger: true);

        return builder;
    }
}
