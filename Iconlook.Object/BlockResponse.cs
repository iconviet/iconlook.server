using System;
using Agiper.Object;

namespace Iconlook.Object
{
    public class BlockResponse : ResponseBase<BlockResponse>
    {
        public int Size { get; set; }
        public float Fee { get; set; }
        public int Height { get; set; }
        public string Hash { get; set; }
        public float Amount { get; set; }
        public int Transactions { get; set; }
        public DateTime Timestamp { get; set; }
        public string ProduderName { get; set; }
        public string ProducerAddress { get; set; }
    }
}