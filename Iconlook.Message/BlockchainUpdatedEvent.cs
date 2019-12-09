using System;
using Agiper.Server;

namespace Iconlook.Message
{
    public class BlockchainUpdatedEvent : EventBase<BlockchainUpdatedEvent>
    {
        public long IcxSupply { get; set; }
        public long BlockHeight { get; set; }
        public long IcxCirculation { get; set; }
        public long TransactionCount { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}