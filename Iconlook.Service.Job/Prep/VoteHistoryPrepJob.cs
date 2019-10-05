using Agiper.Server;
using Serilog;

namespace Iconlook.Service.Job.PRep
{
    public class VoteHistoryPRepJob : JobBase
    {
        public void Run()
        {
            Log.Information("VoteHistoryPRepJob ran");
        }
    }
}