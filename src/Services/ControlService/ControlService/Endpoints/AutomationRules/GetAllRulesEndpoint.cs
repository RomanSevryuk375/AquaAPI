using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Enums;
using BuildingBlocks.Presentation.Endpoints;
using BuildingBlocks.Presentation.ResultExtensions;
using Control.Application.Features.AutomationRules.Queries;
using Control.Application.Features.AutomationRules.Queries.GetAllRules;

namespace Control.API.Endpoints.AutomationRules;

public sealed class GetAllRulesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiConstants.Routes.AutomationRules, async (
            Guid ecosystemId,
            Guid? relayId,
            RuleAction? action,
            Operator? @operator,
            ISender sender,
            IUserContext userContext,
            int skip = 0,
            int take = 10,
            CancellationToken cancellationToken = default) =>
        {
            GetAllRulesQuery query = new GetAllRulesQuery
            {
                UserId = userContext.UserId,
                EcosystemId = ecosystemId,
                RelayId = relayId,
                Action = action,
                Operator = @operator,
                Skip = skip,
                Take = take
            };

            var result = await sender.Send(query, cancellationToken);

            return result.ToIResult();
        })
        .WithTags("Automation Rules")
        .Produces<IReadOnlyList<AutomationRuleDto>>()
        .RequireAuthorization(SubPermissions.AutoRuleCreate);
    }
}