namespace Control.Application.Interfaces;

public interface IScheduleProcessor
{
    Task ProcessAsync(CancellationToken cancellationToken);
}