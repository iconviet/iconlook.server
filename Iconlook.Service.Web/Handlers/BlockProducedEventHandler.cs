using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using DynamicData;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Service.Web.Sources;
using NServiceBus;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockProducedEventHandler : HandlerBase, IHandleMessages<BlockProducedEvent>
    {
        public Task Handle(BlockProducedEvent message, IMessageHandlerContext context)
        {
            Source.Blocks.AddOrUpdate(new BlockResponse
            {
                Height = message.Height,
                Timestamp = message.Timestamp
            });
            Source.Transactions.AddOrUpdate(message.Transactions.Select(x => new TransactionResponse
            {
                Hash = x.Hash,
                Timestamp = x.Timestamp
            }));
            return Task.CompletedTask;
        }
    }
}