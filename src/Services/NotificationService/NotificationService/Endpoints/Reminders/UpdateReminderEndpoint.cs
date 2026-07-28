using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.Reminders.Commands.UpdateReminder;

namespace Notification.API.Endpoints.Reminders;

public sealed class UpdateReminderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut($"{ApiConstants.Routes.Reminders}/{{id:guid}}", async (
            Guid id,
            UpdateReminderCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            UpdateReminderCommand enrichedCommand = command with { ReminderId = id };

            Result result = await sender.Send(enrichedCommand, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Reminders")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.ReminderManage);
    }
}
