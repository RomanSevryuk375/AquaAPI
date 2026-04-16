using Notification.Application.Interfaces;
using Quartz;

namespace Notification.Infrastructure.BackgroundJob;

public class RemienderProcessorJob(
    IReminderProcessor processor) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await processor.ProcessAsync(context.CancellationToken);
    }
}
