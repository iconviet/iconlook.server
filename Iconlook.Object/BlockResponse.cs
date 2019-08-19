using System;
using Agiper.Object;

namespace Iconlook.Object
{
    public class BlockResponse : ResponseBase<BlockResponse>
    {
        public int Size { get; set; }
        public int Height { get; set; }
        public string Hash { get; set; }
        public decimal Fee { get; set; }
        public decimal Amount { get; set; }
        public int Transactions { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}