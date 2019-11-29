using System.Threading.Tasks;
using Agiper.Server;
using Serilog;

namespace Iconlook.Service.Job.PRep
{
    public class VoteHistoryPRepJob : JobBase
    {
        public override Task RunAsync()
        {
            Log.Information("VoteHistoryPRepJob ran");
            return Task.CompletedTask;
        }
    }
}