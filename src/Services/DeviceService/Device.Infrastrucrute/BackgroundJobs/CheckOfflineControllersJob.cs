using Device.Application.Interfaces;
using Quartz;

namespace Device.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class CheckOfflineControllersJob(
    IControllerOfflineCheckerService service) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await service.CheckAndDisableController(context.CancellationToken);
    }
}
