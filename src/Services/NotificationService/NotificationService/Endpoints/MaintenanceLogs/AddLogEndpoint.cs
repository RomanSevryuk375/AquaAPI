using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.MaintenanceLogs.Commands.CreateMaintenanceLog;

namespace Notification.API.Endpoints.MaintenanceLogs;

public sealed class AddLogEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.Routes.MaintenanceLogs, async (
            CreateMaintenanceLogCommand command,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            CreateMaintenanceLogCommand enrichedCommand = command with { UserId = userContext.UserId };

            Result<Guid> result = await sender.Send(enrichedCommand, cancellationToken);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetMaintenanceLogById", new { id = result.Value }, result.Value)
                : result.ToIResult();
        })
        .WithTags("Maintenance Logs")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization(SubPermissions.MaintenanceLogWrite);
    }
}
