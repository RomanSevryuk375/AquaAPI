using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.VacationModes.Queries.GetVacationModeById;
using Control.Application.Features.VacationModes.Queries.Shared;

namespace Control.API.Endpoints.VacationModes;

public sealed class GetVacationModeByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.VacationModes}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            GetVacationModeByIdQuery query = new GetVacationModeByIdQuery
            {
                VacationModeId = id,
                UserId = userContext.UserId
            };

            Result<VacationModeDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetVacationModeById")
        .WithTags("Vacation Modes")
        .Produces<VacationModeDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.VacationMode);
    }
}