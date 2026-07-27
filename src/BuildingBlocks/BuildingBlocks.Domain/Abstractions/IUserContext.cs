namespace BuildingBlocks.Domain.Abstractions;

public interface IUserContext
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
}

