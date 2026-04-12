using Control.Application.Interfaces;
using Quartz;

namespace Control.Infrastructure.BackgroundJobs;

public class ScheduleProcessJob(IScheduleProcessor scheduleProcessor) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await scheduleProcessor.ProcessAsync(context.CancellationToken);
    }
}
