using Quartz;
using Telemetry.Application.Interfaces;

namespace Telemetry.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class CheckSensorStateJob(
    ISensorStateCheckerService service) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await service.CheckStateAndNotify(context.CancellationToken);
    }
}
