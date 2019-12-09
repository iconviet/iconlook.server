using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Message
{
    public class BlockchainUpdatedEvent : EventBase<BlockchainUpdatedEvent>
    {
        public BlockchainResponse Blockchain { get; set; }
    }
}