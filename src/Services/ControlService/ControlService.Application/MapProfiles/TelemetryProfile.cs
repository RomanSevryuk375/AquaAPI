using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Telemetrys;
using Control.Application.Features.Telemetry.Commands.ProcessTelemetry;

namespace Control.Application.MapProfiles;

public sealed class TelemetryProfile : Profile
{
    public TelemetryProfile()
    {
        CreateMap<TelemetryReceivedEvent, ProcessTelemetryCommand>();
    }
}
