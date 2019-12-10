using System.Threading.Tasks;
using Agiper.Server;
using Serilog;

namespace Iconlook.Service.Mon
{
    public class UpdatePeerStatusJob : JobBase
    {
        public override Task RunAsync()
        {
            Log.Information("UpdatePeerStatusJob ran");
            return Task.CompletedTask;
        }
    }
}