using System.Threading.Tasks;
using Iconviet.Server;
using Serilog;

namespace Iconlook.Service.Job.Works
{
    public class UpdateScoreWork : WorkBase
    {
        public override Task StartAsync()
        {
            Log.Information("{Work} started", nameof(UpdateScoreWork));
            return Task.CompletedTask;
        }
    }
}