using System.Threading.Tasks;
using Agiper.Server;
using DynamicData;
using Iconlook.Message;
using Iconlook.Service.Web.Sources;
using NServiceBus;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockProducedEventHandler : HandlerBase, IHandleMessages<BlockProducedEvent>
    {
        public Task Handle(BlockProducedEvent message, IMessageHandlerContext context)
        {
            Source.Blocks.AddOrUpdate(message.Block);
            Source.Transactions.AddOrUpdate(message.Transactions);
            return Task.CompletedTask;
        }
    }
}