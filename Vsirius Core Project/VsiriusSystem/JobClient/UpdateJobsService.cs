using Quartz;

namespace QuartzInfrastructure
{
    public interface IUpdateJobsService
    {
        public Task<bool> UpdateJob<T>(JobInfoDto job) where T : IJob;
    }
    public class UpdateJobsService : IUpdateJobsService
    {
        private readonly ISchedulerFactory _factory;

        public UpdateJobsService(ISchedulerFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> UpdateJob<T>(JobInfoDto job) where T : IJob
        {

            if (job == null)
            {
                return false;
            }
            IScheduler _scheduler = await _factory.GetScheduler();
            var jobKey = JobKey.Create(job.Tittle);

            var jobDetail = await _scheduler.GetJobDetail(jobKey);
            if (jobDetail != null)
            {
                var existingTriggers = await _scheduler.GetTriggersOfJob(jobKey);
                foreach (var trigger in existingTriggers)
                {
                    await _scheduler.UnscheduleJob(trigger.Key);
                }

                if (!job.IsStop)
                {
                    IJobDetail job1 = JobBuilder.Create<T>()
                  .WithIdentity(job.Tittle) // name "myJob", group "group1"
                  .Build();

                    // Trigger the job to run now, and then every 40 seconds
                    ITrigger trigger1 = TriggerBuilder.Create()
                     .WithIdentity(job.Tittle)
                     .StartNow()
                     .WithCronSchedule(job.Cron!)
                     .Build();

                    // Tell Quartz to schedule the job using our trigger
                    await _scheduler.ScheduleJob(job1, trigger1);
                }
            }
            else
            {
                if (!job.IsStop)
                {
                    IJobDetail job1 = JobBuilder.Create<T>()
                  .WithIdentity(job.Tittle) // name "myJob", group "group1"
                  .Build();

                    // Trigger the job to run now, and then every 40 seconds
                    ITrigger trigger1 = TriggerBuilder.Create()
                     .WithIdentity(job.Tittle)
                     .StartNow()
                     .WithCronSchedule(job.Cron!)
                     .Build();

                    // Tell Quartz to schedule the job using our trigger
                    await _scheduler.ScheduleJob(job1, trigger1);
                }
            }
            return true;
        }
    }
}
