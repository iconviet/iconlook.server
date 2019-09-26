using System;
using Agiper.Object;

namespace Iconlook.Object
{
    public class TransactionResponse : ResponseBase<TransactionResponse>
    {
        public int Steps { get; set; }
        public float Fee { get; set; }
        public string To { get; set; }
        public long Block { get; set; }
        public string From { get; set; }
        public byte[] Data { get; set; }
        public string Hash { get; set; }
        public float Amount { get; set; }
        public int StepLimit { get; set; }
        public string Status { get; set; }
        public float StepPrice { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}