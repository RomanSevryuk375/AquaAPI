using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Enums;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.Ecosystems.Queries;
using Control.Application.Features.Ecosystems.Queries.GetAllEcosystems;

namespace Control.API.Endpoints.Ecosystems;

public sealed class GetAllEcosystemsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.Ecosystems, async (
            string? name,
            Guid? controllerId,
            EcosystemType? type,
            ISender sender,
            IUserContext userContext,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default) =>
        {
            GetAllEcosystemsQuery query = new GetAllEcosystemsQuery
            {
                UserId = userContext.UserId,
                Name = name,
                ControllerId = controllerId,
                Type = type,
                Skip = skip,
                Take = take
            };

            var result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Ecosystems")
        .Produces<IReadOnlyList<EcosystemDto>>()
        .RequireAuthorization(SubPermissions.TankRead);
    }
}