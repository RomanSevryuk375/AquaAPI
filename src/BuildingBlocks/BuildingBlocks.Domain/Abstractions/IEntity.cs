namespace BuildingBlocks.Domain.Abstractions;

public interface IEntity
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
}
