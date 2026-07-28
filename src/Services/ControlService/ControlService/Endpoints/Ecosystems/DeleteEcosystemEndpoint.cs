using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.Ecosystems.Commands.DeleteEcosystem;

namespace Control.API.Endpoints.Ecosystems;

public sealed class DeleteEcosystemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ApiConstants.Routes.Ecosystems}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            DeleteEcosystemCommand command = new DeleteEcosystemCommand { EcosystemId = id };

            var result = await sender.Send(command, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Ecosystems")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .RequireAuthorization(SubPermissions.TankDelete);
    }
}