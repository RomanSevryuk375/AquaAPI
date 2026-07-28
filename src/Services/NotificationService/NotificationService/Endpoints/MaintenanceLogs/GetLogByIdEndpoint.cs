using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.MaintenanceLogs.Queries.GetMaintenanceLogById;
using Notification.Application.Features.MaintenanceLogs.Queries.Shared;

namespace Notification.API.Endpoints.MaintenanceLogs;

public sealed class GetLogByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.MaintenanceLogs}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetMaintenanceLogByIdQuery
            {
                Id = id,
                UserId = userContext.UserId
            };

            Result<MaintenanceLogDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetMaintenanceLogById")
        .WithTags("Maintenance Logs")
        .Produces<MaintenanceLogDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.MaintenanceLogRead);
    }
}
