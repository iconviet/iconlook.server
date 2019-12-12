using System.Threading.Tasks;
using Agiper.Server;
using Serilog;

namespace Iconlook.Service.Job
{
    public class UpdatePRepsJob : JobBase
    {
        public override Task RunAsync()
        {
            Log.Information("UpdatePRepsJob ran");
            return Task.CompletedTask;
        }
    }
}