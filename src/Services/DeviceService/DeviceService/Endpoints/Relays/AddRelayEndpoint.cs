using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Relays.Command.AddRelay;
using MediatR;

namespace Device.API.Endpoints.Relays;

public sealed class AddRelayEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.Routes.Relays, async (
            AddRelayCommand command,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            AddRelayCommand commandWithUser = command with { UserId = userContext.UserId };
            Result<RelayCreatedResponse> result = await sender.Send(commandWithUser, cancellationToken);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetRelayById", new { id = result.Value.Id }, result.Value)
                : result.ToIResult();
        })
        .WithTags("Relays")
        .Produces<RelayCreatedResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
