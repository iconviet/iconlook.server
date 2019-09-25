using System.Threading.Tasks;
using Agiper.Server;
using DynamicData;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Service.Web.Sources;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockchainUpdatedEventHandler : HandlerBase, IHandleMessages<BlockchainUpdatedEvent>
    {
        public Task Handle(BlockchainUpdatedEvent message, IMessageHandlerContext context)
        {
            Source.Blockchain.AddOrUpdate(new BlockchainResponse
            {
                BlockHeight = message.BlockHeight,
                TotalTransactions = message.TotalTransactions
            });
            Log.Information("BlockchainUpdatedEvent received");
            return Task.CompletedTask;
        }
    }
}