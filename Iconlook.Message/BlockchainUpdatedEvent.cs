using System;
using Agiper.Server;

namespace Iconlook.Message
{
    public class BlockchainUpdatedEvent : EventBase<BlockchainUpdatedEvent>
    {
        public long BlockHeight { get; set; }
        public long TokenSupply { get; set; }
        public long TransactionCount { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}