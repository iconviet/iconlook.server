using System.Threading.Tasks;
using Agiper.Server;
using DynamicData;
using Iconlook.Message;
using Iconlook.Service.Web.Sources;
using NServiceBus;

namespace Iconlook.Service.Web.Handlers
{
    public class ChainUpdatedEventHandler : HandlerBase, IHandleMessages<ChainUpdatedEvent>
    {
        public Task Handle(ChainUpdatedEvent message, IMessageHandlerContext context)
        {
            Source.Chain.AddOrUpdate(message.Chain);
            return Task.CompletedTask;
        }
    }
}