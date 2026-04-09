namespace Control.Domain.Interfaces;

public interface IEntity
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
}
