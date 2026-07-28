using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.VacationModes.Commands.CreateVacationMode;

namespace Control.API.Endpoints.VacationModes;

public sealed class CreateVacationModeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiConstants.Routes.VacationModes, async (
            CreateVacationModeCommand command,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            Result<Guid> result = await sender.Send(command, cancellationToken);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetVacationModeById", new { id = result.Value }, result.Value)
                : result.ToIResult();
        })
        .WithTags("Vacation Modes")
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .RequireAuthorization(SubPermissions.VacationMode);
    }
}