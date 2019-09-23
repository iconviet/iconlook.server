using System.Reflection;
using Hangfire;
using Iconlook.Server;
using Iconlook.Service.Job.Prep;

namespace Iconlook.Service.Job
{
    public class JobHost : Host
    {
        public JobHost() : base("Job", typeof(JobHost).Assembly)
        {
        }

        public JobHost(string name, Assembly assembly) : base(name, assembly)
        {
        }

        protected override void OnStart()
        {
            BackgroundJob.Enqueue<VoteHistoryPrepJob>(x => x.Run());
            RecurringJob.AddOrUpdate<VoteHistoryPrepJob>(x => x.Run(), Cron.Minutely());
        }
    }
}