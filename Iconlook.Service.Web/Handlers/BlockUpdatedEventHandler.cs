using System.Threading.Tasks;
using Agiper.Server;
using DynamicData;
using Iconlook.Message;
using Iconlook.Service.Web.Sources;
using NServiceBus;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockUpdatedEventHandler : HandlerBase, IHandleMessages<BlockUpdatedEvent>
    {
        public Task Handle(BlockUpdatedEvent message, IMessageHandlerContext context)
        {
            Source.Blocks.AddOrUpdate(message.Block);
            Source.Transactions.AddOrUpdate(message.Transactions);
            return Task.CompletedTask;
        }
    }
}