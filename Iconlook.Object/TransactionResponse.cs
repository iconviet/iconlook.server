using System;
using Agiper.Object;

namespace Iconlook.Object
{
    public class TransactionResponse : ResponseBase<TransactionResponse>
    {
        public int Steps { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Hash { get; set; }
        public byte[] Data { get; set; }
        public decimal Fee { get; set; }
        public int StepLimit { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public int BlockHeight { get; set; }
        public decimal StepPrice { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}