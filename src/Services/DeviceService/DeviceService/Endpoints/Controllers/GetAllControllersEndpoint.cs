using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Device.Application.Features.Controllers.Query.GetAllControllers;
using Device.Application.Features.Controllers.Query.Shared;
using MediatR;

namespace Device.API.Endpoints.Controllers;

public sealed class GetAllControllersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.Controllers, async (
            string? searchTerm,
            bool? isOnline,
            ISender sender,
            IUserContext userContext,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllControllersQuery
            {
                UserId = userContext.UserId,
                SearchTerm = searchTerm,
                IsOnline = isOnline,
                Skip = skip,
                Take = take
            };

            Result<IReadOnlyList<ControllerDto>> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Controllers")
        .Produces<IReadOnlyList<ControllerDto>>()
        .RequireAuthorization(SubPermissions.DeviceControl);
    }
}
