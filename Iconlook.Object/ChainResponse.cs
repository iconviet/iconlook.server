using System;
using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class ChainResponse : ResponseBase<ChainResponse>
    {
        public long MarketCap { get; set; }
        public long IcxSupply { get; set; }
        public double IcxPrice { get; set; }
        public long BlockHeight { get; set; }
        public long TotalStaked { get; set; }
        public long TotalUnstaking { get; set; }
        public long TotalDelegated { get; set; }
        public long IcxCirculation { get; set; }
        public long PublicTreasury { get; set; }
        public long TransactionCount { get; set; }
        public long StakingAddressCount { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public long UnstakingAddressCount { get; set; }
        public double StakedPercentage => (double) TotalStaked / IcxSupply;
        public double DelegatedPercentage => (double) TotalDelegated / IcxSupply;
        
        protected override void AddRules(Validator<ChainResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.BlockHeight).GreaterThan(0);
        }
    }
}