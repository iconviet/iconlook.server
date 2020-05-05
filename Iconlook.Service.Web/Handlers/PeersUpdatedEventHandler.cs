using System.Threading.Tasks;
using Iconviet.Server;
using Iconlook.Message;
using NServiceBus;
using System.Linq;

namespace Iconlook.Service.Web.Handlers
{
    public class PeersUpdatedEventHandler : HandlerBase, IHandleMessages<PeersUpdatedEvent>
    {
        public Task Handle(PeersUpdatedEvent message, IMessageHandlerContext context)
        {
            LocalCache.LastPeerResponse = message.Busy.FirstOrDefault();
            return Task.CompletedTask;
        }
    }
}