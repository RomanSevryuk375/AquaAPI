using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.Notifications.Queries.GetNotificationById;
using Notification.Application.Features.Notifications.Queries.Shared;

namespace Notification.API.Endpoints.Notifications;

public sealed class GetNotificationByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.Notifications}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetNotificationByIdQuery
            {
                NotificationId = id,
                UserId = userContext.UserId
            };

            Result<NotificationDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetNotificationById")
        .WithTags("Notifications")
        .Produces<NotificationDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
