using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.DTOs.Ecosystems;
using Control.Application.Features.Ecosystems.Commands.CreateEcosystem;

namespace Control.API.Endpoints.Ecosystems;

public sealed class CreateEcosystemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.Routes.Ecosystems, async (
            EcosystemRequestDto request,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            CreateEcosystemCommand command = new CreateEcosystemCommand
            {
                Type = request.Type,
                Name = request.Name,
                Volume = request.Volume,
                ControllerId = request.ControllerId,
                UserId = userContext.UserId
            };

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetEcosystemById", new { id = result.Value }, result.Value)
                : result.ToIResult();
        })
        .WithTags("Ecosystems")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization(SubPermissions.TankCreate);
    }
}