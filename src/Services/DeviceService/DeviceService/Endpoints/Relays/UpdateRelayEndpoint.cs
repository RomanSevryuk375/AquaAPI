using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Relays.Command.UpdateRelay;
using MediatR;

namespace Device.API.Endpoints.Relays;

public sealed class UpdateRelayEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ApiConstants.Routes.Relays}/{{id:guid}}", async (
            Guid id,
            UpdateRelayCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            UpdateRelayCommand enrichedCommand = command with { RelayId = id };

            Result result = await sender.Send(enrichedCommand, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Relays")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
