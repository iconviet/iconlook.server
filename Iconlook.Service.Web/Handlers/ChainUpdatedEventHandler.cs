using System.Threading.Tasks;
using Iconviet.Server;
using Iconlook.Message;
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