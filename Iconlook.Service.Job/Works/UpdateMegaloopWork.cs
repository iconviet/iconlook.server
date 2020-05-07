using System.Threading.Tasks;
using Iconviet.Server;
using Serilog;

namespace Iconlook.Service.Job.Works
{
    public class UpdateMegaloopWork : WorkBase
    {
        public override Task StartAsync()
        {
            Log.Information("{Work} started", nameof(UpdateMegaloopWork));
            return Task.CompletedTask;
        }
    }
}