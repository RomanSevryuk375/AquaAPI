using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.Reminders.Queries.GetReminderById;
using Notification.Application.Features.Reminders.Queries.Shared;

namespace Notification.API.Endpoints.Reminders;

public sealed class GetReminderByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.Reminders}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetReminderByIdQuery
            {
                ReminderId = id,
                UserId = userContext.UserId
            };

            Result<ReminderDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetReminderById")
        .WithTags("Reminders")
        .Produces<ReminderDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.ReminderManage);
    }
}
