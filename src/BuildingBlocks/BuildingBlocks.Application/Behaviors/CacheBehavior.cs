using BuildingBlocks.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using ZiggyCreatures.Caching.Fusion;

namespace BuildingBlocks.Application.Behaviors;

public sealed class CacheBehavior<TRequest, TResponse>(
    IFusionCache cache,
    ILogger<CacheBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Evaluating cache for key: {CacheKey}", request.CacheKey);

        var setup = new FusionCacheEntryOptions
        {
            Duration = request.Expiration ?? TimeSpan.FromSeconds(5),
            IsFailSafeEnabled = request.AllowFailSafe,
            FailSafeMaxDuration = TimeSpan.FromHours(2)
        };

        TResponse? response = await cache.GetOrSetAsync<TResponse>(
            request.CacheKey,
            async (ctx, ct) =>
            {
                logger.LogInformation("Cache Miss for {CacheKey}. Executing database query...", request.CacheKey);
                return await next();
            },
            setup,
            token: cancellationToken);

        return response;
    }
}
