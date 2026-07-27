using BuildingBlocks.Domain.Results;
using MediatR;

namespace BuildingBlocks.Domain.Abstractions;

public interface IBaseCommand { }

public interface ICommand : IRequest<Result>, IBaseCommand { }

public interface ICommand<TValue> : IRequest<Result<TValue>>, IBaseCommand { }

public interface IQuery<out TResponse> : IRequest<TResponse> { }
