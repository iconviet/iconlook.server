using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Message;
using Iconlook.Service.Web.Models;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockchainUpdatedEventHandler : HandlerBase, IHandleMessages<BlockchainUpdatedEvent>
    {
        public Task Handle(BlockchainUpdatedEvent message, IMessageHandlerContext context)
        {
            BlockchainModel.BlockHeight += 1;
            BlockchainModel.AllTransactions += 10;
            Log.Information("BlockchainUpdatedEvent received");
            return Task.CompletedTask;
        }
    }
}