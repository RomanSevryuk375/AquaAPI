using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.Reminders.Commands.DeleteReminder;

namespace Notification.API.Endpoints.Reminders;

public sealed class DeleteReminderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{ApiConstants.Routes.Reminders}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var command = new DeleteReminderCommand { ReminderId = id };

            Result result = await sender.Send(command, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Reminders")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.ReminderManage);
    }
}
