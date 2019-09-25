using Agiper.Server;

namespace Iconlook.Message
{
    public class BlockchainUpdatedEvent : EventBase<BlockchainUpdatedEvent>
    {
        public long BlockHeight { get; set; }
        public long TokenSupply { get; set; }
        public long TotalTransactions { get; set; }
    }
}