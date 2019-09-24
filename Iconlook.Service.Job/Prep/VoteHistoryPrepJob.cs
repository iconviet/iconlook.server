using Agiper.Server;
using Serilog;

namespace Iconlook.Service.Job.Prep
{
    public class VoteHistoryPrepJob : JobBase
    {
        public void Run()
        {
            Log.Information("VoteHistoryPrepJob ran");
        }
    }
}