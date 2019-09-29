using System;

namespace Iconlook.Object
{
    public class BlockchainResponse
    {
        public long MarketCap { get; set; }
        public long BlockHeight { get; set; }
        public long TokenSupply { get; set; }
        public double TokenPrice { get; set; }
        public long TokenCirculation { get; set; }
        public long TotalTransactions { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}