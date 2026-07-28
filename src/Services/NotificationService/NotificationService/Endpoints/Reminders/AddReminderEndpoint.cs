using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.Reminders.Commands.CreateReminder;

namespace Notification.API.Endpoints.Reminders;

public sealed class AddReminderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.Routes.Reminders, async (
            CreateReminderCommand command,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            CreateReminderCommand enrichedCommand = command with { UserId = userContext.UserId };

            Result<Guid> result = await sender.Send(enrichedCommand, cancellationToken);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetReminderById", new { id = result.Value }, result.Value)
                : result.ToIResult();
        })
        .WithTags("Reminders")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization(SubPermissions.ReminderManage);
    }
}
