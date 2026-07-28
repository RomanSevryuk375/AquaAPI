using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Enums;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.Notifications.Queries.GetAllNotifications;
using Notification.Application.Features.Notifications.Queries.Shared;

namespace Notification.API.Endpoints.Notifications;

public sealed class GetAllNotificationsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.Notifications, async (
            Guid? ecosystemId,
            NotificationLevel? level,
            bool? isRead,
            string? searchTerm,
            ISender sender,
            IUserContext userContext,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllNotificationsQuery
            {
                UserId = userContext.UserId,
                EcosystemId = ecosystemId,
                Level = level,
                IsRead = isRead,
                SearchTerm = searchTerm,
                Skip = skip,
                Take = take
            };

            Result<IReadOnlyList<NotificationDto>> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Notifications")
        .Produces<IReadOnlyList<NotificationDto>>()
        .RequireAuthorization();
    }
}
