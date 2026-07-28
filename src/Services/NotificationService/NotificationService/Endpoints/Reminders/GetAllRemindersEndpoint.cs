using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Authorization;
using BuildingBlocks.Presentation.Constants;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using MediatR;
using Notification.Application.Features.Reminders.Queries.GetAllReminders;
using Notification.Application.Features.Reminders.Queries.Shared;

namespace Notification.API.Endpoints.Reminders;

public sealed class GetAllRemindersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.Reminders, async (
            Guid? ecosystemId,
            string? searchTerm,
            DateTime? nextDueAtFrom,
            DateTime? nextDueAtTo,
            ISender sender,
            IUserContext userContext,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default) =>
        {
            var query = new GetAllRemindersQuery
            {
                UserId = userContext.UserId,
                EcosystemId = ecosystemId,
                SearchTerm = searchTerm,
                NextDueAtFrom = nextDueAtFrom,
                NextDueAtTo = nextDueAtTo,
                Skip = skip,
                Take = take
            };

            Result<IReadOnlyList<ReminderDto>> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Reminders")
        .Produces<IReadOnlyList<ReminderDto>>()
        .RequireAuthorization(SubPermissions.ReminderManage);
    }
}
