using System.Linq;
using System.Threading.Tasks;
using Iconlook.Message;
using Iconviet.Server;
using NServiceBus;

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