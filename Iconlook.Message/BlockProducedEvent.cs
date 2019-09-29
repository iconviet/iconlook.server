using System;
using System.Collections.Generic;
using Agiper.Server;
using Iconlook.Entity;

namespace Iconlook.Message
{
    public class BlockProducedEvent : EventBase<BlockProducedEvent>
    {
        public long Height { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }

        public BlockProducedEvent()
        {
            Transactions = new List<Transaction>();
        }
    }
}