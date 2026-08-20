namespace BuildingBlocks.Domain.Abstractions;

public interface IUserBoundRequest
{
    public Guid UserId { get; }
}
