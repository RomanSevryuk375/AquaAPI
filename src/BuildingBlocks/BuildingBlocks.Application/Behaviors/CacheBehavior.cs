using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
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

        MaybeValue<TResponse> cachedValue = await cache.TryGetAsync<TResponse>(
            request.CacheKey,
            setup,
            token: cancellationToken);

        if (cachedValue.HasValue)
        {
            logger.LogDebug("Cache Hit for {CacheKey}", request.CacheKey);
            return cachedValue.Value;
        }

        logger.LogInformation("Cache Miss for {CacheKey}. Executing database query...", request.CacheKey);
        TResponse response = await next();

        if (response is Result { IsFailure: true })
        {
            return response;
        }

        await cache.SetAsync(
            request.CacheKey,
            response,
            setup,
            token: cancellationToken);

        return response;
    }
}
