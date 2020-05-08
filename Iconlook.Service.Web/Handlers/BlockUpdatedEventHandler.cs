using System.Threading.Tasks;
using Iconlook.Message;
using Iconviet.Server;
using NServiceBus;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockUpdatedEventHandler : HandlerBase, IHandleMessages<BlockUpdatedEvent>
    {
        public Task Handle(BlockUpdatedEvent message, IMessageHandlerContext context)
        {
            LocalCache.LastBlockResponse = message.Block;
            return Task.CompletedTask;
        }
    }
}