using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.Presentation.Endpoints;

public interface IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app);
}
