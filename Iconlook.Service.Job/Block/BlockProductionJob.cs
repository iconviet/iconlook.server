using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Message;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Job.Block
{
    public class BlockProductionJob : JobBase
    {
        public Task Run()
        {
            Log.Information("BlockProductionJob ran");
            return Endpoint.Publish(new BlockProducedEvent());
        }
    }
}