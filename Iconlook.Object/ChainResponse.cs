using System;
using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class ChainResponse : ResponseBase<ChainResponse>
    {
        // ReSharper disable InconsistentNaming
        public decimal IRep { get; set; }
        // ReSharper restore InconsistentNaming
        public long MarketCap { get; set; }
        public long IcxSupply { get; set; }
        public long BlockHeight { get; set; }
        public long TotalStaked { get; set; }
        public decimal IcxPrice { get; set; }
        public long TotalUnstaking { get; set; }
        public long TotalDelegated { get; set; }
        public long IcxCirculation { get; set; }
        public long PublicTreasury { get; set; }
        public long TransactionCount { get; set; }
        public double RRepPercentage { get; set; }
        public double StakedPercentage { get; set; }
        public long NextTermBlockHeight { get; set; }
        public string NextTermLocalTime { get; set; }
        public string NextTermCountdown { get; set; }
        public long StakingAddressCount { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public long UnstakingAddressCount { get; set; }
        public double DelegatedPercentage { get; set; }

        protected override void AddRules(Validator<ChainResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.BlockHeight).GreaterThan(0);
        }
    }
}