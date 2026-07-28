using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.Reminders.Commands.CompleteReminder;

namespace Notification.API.Endpoints.Reminders;

public sealed class CompleteReminderEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch($"{ApiConstants.Routes.Reminders}/{{id:guid}}/complete", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            var command = new CompleteReminderCommand { ReminderId = id };

            Result result = await sender.Send(command, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Reminders")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.ReminderManage);
    }
}
