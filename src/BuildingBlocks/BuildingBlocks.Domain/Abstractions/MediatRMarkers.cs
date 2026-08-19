using BuildingBlocks.Domain.Results;
using MediatR;

namespace BuildingBlocks.Domain.Abstractions;

public interface IBaseCommand { }

public interface ICommand : IRequest<Result>, IBaseCommand { }

public interface ICommand<TValue> : IRequest<Result<TValue>>, IBaseCommand { }

public interface IQuery<out TResponse> : IRequest<TResponse> { }

public interface ICachedQuery<TResponse> : IRequest<TResponse>
{
    public string CacheKey { get; }
    public TimeSpan? Expiration { get; }
    public bool AllowFailSafe => true;
}
