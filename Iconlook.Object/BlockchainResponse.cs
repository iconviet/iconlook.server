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
        public long TotalStaked { get; set; }
        public long TotalDelegated { get; set; }
        public long IcxCirculation { get; set; }
        public long PublicTreasury { get; set; }
        public long TransactionCount { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<BlockchainResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.MarketCap).GreaterThan(0);
            validator.RuleFor(x => x.IcxSupply).GreaterThan(0);
            validator.RuleFor(x => x.BlockHeight).GreaterThan(0);
            validator.RuleFor(x => x.TotalStaked).GreaterThan(0);
            validator.RuleFor(x => x.TotalDelegated).GreaterThan(0);
            validator.RuleFor(x => x.IcxCirculation).GreaterThan(0);
            validator.RuleFor(x => x.PublicTreasury).GreaterThan(0);
            validator.RuleFor(x => x.TransactionCount).GreaterThan(0);
        }
    }
}