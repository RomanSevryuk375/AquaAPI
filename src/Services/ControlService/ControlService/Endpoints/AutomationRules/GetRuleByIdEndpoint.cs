using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.AutomationRules.Queries;
using Control.Application.Features.AutomationRules.Queries.GetRuleById;

namespace Control.API.Endpoints.AutomationRules;

public sealed class GetRuleByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{ApiConstants.Routes.AutomationRules}/{{id:guid}}", async (
            Guid id,
            ISender sender,
            IUserContext userContext,
            CancellationToken cancellationToken = default) =>
        {
            GetRuleByIdQuery query = new GetRuleByIdQuery
            {
                RuleId = id,
                UserId = userContext.UserId
            };

            Result<AutomationRuleDto> result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithName("GetRuleById")
        .WithTags("Automation Rules")
        .Produces<AutomationRuleDto>()
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization(SubPermissions.AutoRuleCreate);
    }
}