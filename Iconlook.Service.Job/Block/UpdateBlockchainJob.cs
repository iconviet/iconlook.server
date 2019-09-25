using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Message;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Job.Block
{
    public class UpdateBlockchainJob : JobBase
    {
        public Task Run()
        {
            Log.Information("UpdateBlockchainJob ran");
            return Endpoint.Publish(new BlockchainUpdatedEvent());
        }
    }
}