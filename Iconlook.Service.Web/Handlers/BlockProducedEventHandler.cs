using Agiper.Server;
using DynamicData;
using Iconlook.Message;
using Iconlook.Service.Web.Sources;
using NServiceBus;
using System.Threading.Tasks;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockProducedEventHandler : HandlerBase, IHandleMessages<BlockProducedEvent>
    {
        public Task Handle(BlockProducedEvent message, IMessageHandlerContext context)
        {
            Source.Blocks.AddOrUpdate(message.Block.ToResponse());
            Source.Transactions.AddOrUpdate(message.Transactions.ConvertAll(x => x.ToResponse()));
            return Task.CompletedTask;
        }
    }
}