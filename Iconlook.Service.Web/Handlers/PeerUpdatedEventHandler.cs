using System.Threading.Tasks;
using Iconviet.Server;
using Iconlook.Message;
using NServiceBus;

namespace Iconlook.Service.Web.Handlers
{
    public class PeerUpdatedEventHandler : HandlerBase, IHandleMessages<PeerUpdatedEvent>
    {
        public Task Handle(PeerUpdatedEvent message, IMessageHandlerContext context)
        {
            LocalCache.LastPeerResponse = message.Peer;
            return Task.CompletedTask;
        }
    }
}