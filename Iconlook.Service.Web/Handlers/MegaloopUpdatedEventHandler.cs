using System.Threading.Tasks;
using Iconlook.Message;
using Iconviet.Server;
using NServiceBus;

namespace Iconlook.Service.Web.Handlers
{
    public class MegaloopUpdatedEventHandler : HandlerBase, IHandleMessages<MegaloopUpdatedEvent>
    {
        public Task Handle(MegaloopUpdatedEvent message, IMessageHandlerContext context)
        {
            LocalCache.LastMegaloopResponse = message.Megaloop;
            return Task.CompletedTask;
        }
    }
}