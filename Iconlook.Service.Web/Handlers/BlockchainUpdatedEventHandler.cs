using System.Threading.Tasks;
using Agiper.Server;
using DynamicData;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Service.Web.Sources;
using NServiceBus;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockchainUpdatedEventHandler : HandlerBase, IHandleMessages<BlockchainUpdatedEvent>
    {
        public Task Handle(BlockchainUpdatedEvent message, IMessageHandlerContext context)
        {
            Source.Blockchain.AddOrUpdate(new BlockchainResponse
            {
                Timestamp = message.Timestamp,
                BlockHeight = message.BlockHeight,
                TransactionCount = message.TransactionCount
            });
            return Task.CompletedTask;
        }
    }
}