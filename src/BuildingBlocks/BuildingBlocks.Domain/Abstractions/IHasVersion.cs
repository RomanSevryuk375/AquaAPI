namespace BuildingBlocks.Domain.Abstractions;

public interface IHasVersion
{
    public Guid Version { get; }
}
