using System.Threading.Tasks;
using Iconlook.Message;
using Iconviet.Server;
using NServiceBus;

namespace Iconlook.Service.Web.Handlers
{
    public class ChainUpdatedEventHandler : HandlerBase, IHandleMessages<ChainUpdatedEvent>
    {
        public Task Handle(ChainUpdatedEvent message, IMessageHandlerContext context)
        {
            LocalCache.LastChainResponse = message.Chain;
            return Task.CompletedTask;
        }
    }
}