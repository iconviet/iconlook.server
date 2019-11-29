using Agiper.Server;
using System;

namespace Iconlook.Message
{
    public class BlockchainUpdatedEvent : EventBase<BlockchainUpdatedEvent>
    {
        public long BlockHeight { get; set; }
        public long TokenSupply { get; set; }
        public long TotalTransactions { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}