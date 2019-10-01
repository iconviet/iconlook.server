using System;
using Agiper.Object;
using FluentValidation;

namespace Iconlook.Object
{
    public class BlockchainResponse : ResponseBase<BlockchainResponse>
    {
        public long MarketCap { get; set; }
        public long BlockHeight { get; set; }
        public long TokenSupply { get; set; }
        public double TokenPrice { get; set; }
        public long TokenCirculation { get; set; }
        public long TotalTransactions { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        protected override void AddRules(Validator<BlockchainResponse> validator)
        {
            base.AddRules(validator);
            validator.RuleFor(x => x.BlockHeight).GreaterThan(0);
        }
    }
}