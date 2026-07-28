using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.Ecosystems.Queries;
using Control.Application.Features.Ecosystems.Queries.GetEcosystemById;

namespace Control.API.Endpoints.Ecosystems;

public sealed class GetEcosystemByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.Ecosystems}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            GetEcosystemByIdQuery query = new GetEcosystemByIdQuery
            {
                EcosystemId = id,
                UserId = userContext.UserId
            };

            Result<EcosystemDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetEcosystemById")
        .WithTags("Ecosystems")
        .Produces<EcosystemDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.TankRead);
    }
}