using System;
using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class BlockchainResponse : ResponseBase<BlockchainResponse>
    {
        public long MarketCap { get; set; }
        public long IcxSupply { get; set; }
        public double IcxPrice { get; set; }
        public long BlockHeight { get; set; }
        public long IcxCirculation { get; set; }
        public long TransactionCount { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<BlockchainResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.BlockHeight).GreaterThan(0);
        }
    }
}