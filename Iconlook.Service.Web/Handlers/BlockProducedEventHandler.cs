using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Message;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockProducedEventHandler : HandlerBase, IHandleMessages<BlockProducedEvent>
    {
        public Task Handle(BlockProducedEvent message, IMessageHandlerContext context)
        {
            Log.Information("BlockProducedEvent received");
            return Task.CompletedTask;
        }
    }
}