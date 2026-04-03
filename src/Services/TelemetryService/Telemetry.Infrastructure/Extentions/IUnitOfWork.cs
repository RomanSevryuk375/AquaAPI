namespace Telemetry.Infrastructure.Extentions
{
    public interface IUnitOfWork
    {
        void Dispose();
        Task SaveChanges(CancellationToken cancellationToken);
    }
}