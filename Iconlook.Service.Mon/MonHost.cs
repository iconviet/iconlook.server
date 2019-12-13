using System.Reflection;
using Hangfire;
using Iconlook.Server;
using Iconlook.Service.Job;

namespace Iconlook.Service.Mon
{
    public class MonHost : Host
    {
        public MonHost() : base("Mon", typeof(MonHost).Assembly)
        {
        }

        public MonHost(string name, Assembly assembly) : base(name, assembly)
        {
        }

        protected override void OnStart()
        {
            BackgroundJob.Enqueue<UpdatePeersJob>(x => x.RunAsync());
            RecurringJob.AddOrUpdate<UpdatePeersJob>(x => x.RunAsync(), "* * * ? * *");
        }
    }
}