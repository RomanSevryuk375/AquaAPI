using MediatR;

namespace BuildingBlocks.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    public DateTime OccurredOn { get; }
}
