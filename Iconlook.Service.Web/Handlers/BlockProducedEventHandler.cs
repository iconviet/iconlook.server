using System.Threading.Tasks;
using Agiper.Server;
using DynamicData;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Service.Web.Sources;
using NServiceBus;
using ServiceStack;

namespace Iconlook.Service.Web.Handlers
{
    public class BlockProducedEventHandler : HandlerBase, IHandleMessages<BlockProducedEvent>
    {
        public Task Handle(BlockProducedEvent message, IMessageHandlerContext context)
        {
            Source.Blocks.AddOrUpdate(message.ConvertTo<BlockResponse>());
            Source.Transactions.AddOrUpdate(message.Transactions.ConvertAll(x => x.ConvertTo<TransactionResponse>()));
            return Task.CompletedTask;
        }
    }
}